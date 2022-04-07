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
using DomainModels.Models;
using FysioAppUX;
using DomainServices.Repos;
using InfrastructureAPIHandler.Data;
using DatabaseHandler.Data;

namespace FysioAppUX
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddAuthorization();

            services.AddDbContext<FysioIdentityDBContext>(config => config.UseSqlServer(Configuration.GetConnectionString("IdentityDB")));

            services.AddDbContext<FysioDataContext>(config => config.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
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
                AuthorizationPolicyBuilder defaultAuthBuilder = new();
                var defaultAuthPolicy = defaultAuthBuilder
                .RequireAuthenticatedUser()
                .RequireClaim(ClaimTypes.DateOfBirth)
                .Build();

                config.AddPolicy("admin", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"));
                config.AddPolicy("Claim.DoB", policyBuilder =>
                {
                    policyBuilder.AddRequirements(new CustomRequireClaim(ClaimTypes.DateOfBirth));
                });
            });

            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();

            services.AddScoped<IPatient, PatientRepository>();
            services.AddScoped<IActionPlan, ActionPlanRepository>();
            services.AddScoped<IAdress, AdressRepository>();
            services.AddScoped<IComment, CommentRepositroy>();
            services.AddScoped<IFysioWorker, FysioWorkerRepositroy>();
            services.AddScoped<IPatientFile, PatientFileRepository>();
            services.AddScoped<ITherapySession, TherapySessionRepository>();
            services.AddScoped<IGraphQLClient>(s => new GraphQLHttpClient(Configuration["GraphQLURI"], new NewtonsoftJsonSerializer()));
            services.AddScoped<OwnerConsumer>();
            services.AddScoped<IDiagnose, DiagnoseRepository>();
            services.AddScoped<ITreatment, TreatmentRepository>();
            services.AddControllersWithViews();
            services.AddScoped<TotalPatients>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
