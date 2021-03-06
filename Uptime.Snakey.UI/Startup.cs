using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uptime.Snakey.Service;
using Uptime.Snakey.Service.Interfaces;

namespace Uptime.Snakey.UI
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc(option => option.EnableEndpointRouting = false)
                .AddNewtonsoftJson();
            services.AddScoped<IRssFeedService, RssFeedService>();


            services.AddScoped<IMercuryApiService, MercuryApiService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            /*app.UseRouting(routes =>
            {
                routes.MapApplication();
                routes.MapControllerRoute(
                    name: "default",
                    template: "{controller=Snakey}/{action=Index}/{id?}");
            });*/

            app.UseCookiePolicy();

            app.UseAuthorization();

            app.UseMvcWithDefaultRoute();
        }
    }
}
