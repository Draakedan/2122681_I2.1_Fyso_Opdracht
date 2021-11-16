using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DatabaseHandler.Models;
using FysioAppUX.Components;
using FysioAppUX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using FysioAppUX.AuthorizationRequirements;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using FysioAppUX;

namespace FysioAppUX
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration; 
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddAuthorization();

            //try
            //{
                services.AddDbContext<FysioIdentityDBContext>(config =>
                {
                    config.UseSqlServer();
                });

                services.AddIdentity<IdentityUser, IdentityRole>(config =>
                {
                    //config.SignIn.RequireConfirmedEmail = true;
                })
                    .AddEntityFrameworkStores<FysioIdentityDBContext>()
                    .AddRoles<IdentityRole>()
                    .AddDefaultTokenProviders();

                services.ConfigureApplicationCookie(config =>
                {
                    config.Cookie.Name = "Identity.Cookie";
                    config.LoginPath = "/Account/Login";
                });

                services.AddAuthorization(config =>
                {
                    AuthorizationPolicyBuilder defaultAuthBuilder = new AuthorizationPolicyBuilder();
                    var defaultAuthPolicy = defaultAuthBuilder
                    .RequireAuthenticatedUser()
                    .RequireClaim(ClaimTypes.DateOfBirth)
                    .Build();

                    config.AddPolicy("admin", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"));
                    //config.AddPolicy("PhysicalTherapist")
                    config.AddPolicy("Claim.DoB", policyBuilder =>
                        {
                            policyBuilder.AddRequirements(new CustomRequireClaim(ClaimTypes.DateOfBirth));
                        });
                });

                services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();
            //}
            //catch { }

            //services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddEntityFrameworkStores<FysioIdentityDBContext>()
            //    .AddDefaultTokenProviders();


            //services.AddAuthentication("PhysicalTherapistAuth")
            //    .AddCookie("PhysicalTherapistAuth", config =>
            //    {
            //        config.Cookie.Name = "PhysicalTherpaist";
            //    });

            services.AddSingleton<IRepository<Patient>, PatientRepository>();
            services.AddSingleton<IRepository<ActionPlan>, ActionPlanRepository>();
            services.AddSingleton<IRepository<Adress>, AdressRepository>();
            services.AddSingleton<IRepository<Comment>, CommentRepositroy>();
            services.AddSingleton<IRepository<FysioWorker>, FysioWorkerRepositroy>();
            services.AddSingleton<IRepository<PatientFile>, PatientFileRepository>();
            services.AddSingleton<IRepository<TherapySession>, TherapySessionRepository>();
            services.AddScoped<IGraphQLClient>(s => new GraphQLHttpClient(Configuration["GraphQLURI"], new NewtonsoftJsonSerializer()));
            services.AddScoped<OwnerConsumer>();
            services.AddControllersWithViews();
            services.AddSingleton<DataReciever>();
            services.AddSingleton<TotalPatients>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
