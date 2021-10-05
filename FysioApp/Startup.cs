using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FysioApp.Models;
using FysioApp.Components;
using FysioApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp
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
            services.AddSingleton<IRepository<Patient>, PatientRepository>();
            services.AddSingleton<IRepository<ActionPlan>, ActionPlanRepository>();
            services.AddSingleton<IRepository<Adress>, AdressRepository>();
            services.AddSingleton<IRepository<Comment>, CommentRepositroy>();
            services.AddSingleton<IRepository<FysioWorker>, FysioWorkerRepositroy>();
            services.AddSingleton<IRepository<PatientFile>, PatientFileRepository>();
            services.AddSingleton<IRepository<TherapySession>, TherapySessionRepository>();
            services.AddControllersWithViews();
            services.AddSingleton<DataReviever>();
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
