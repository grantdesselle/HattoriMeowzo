using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatToyWebApp.Hardware;

namespace CatToyWebApp
{
    public class Startup
    {
        public static bool IsShuttingDown { get; private set; } = false;

        public MyCatToy HattoriMeowzo { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            // initialize cat toy after everthing has started 
            appLifetime.ApplicationStarted.Register(OnStarted);

            appLifetime.ApplicationStopping.Register(OnStopping);
        }

        // added the below method to start cat toy after app is ready
        private void OnStarted()
        {
            HattoriMeowzo = new MyCatToy();
            IsShuttingDown = false;
        }

        private void OnStopping()
        {
            /*
            if(HattoriMeowzo != null)
                HattoriMeowzo.CatCamera.CleanUpCamera();
            */ // this doesn't work, but I think I should be cleaning up the camera here, need to do before camera is disposed
            IsShuttingDown = true;
            MyCatToy.CatToyIsPlaying = false;
        }
    }
}
