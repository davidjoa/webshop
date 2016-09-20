using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Lesson6.Interfaces;
using Microsoft.EntityFrameworkCore;
using Lesson6.Models;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;

namespace Lesson6
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                //builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.

            var connection = @"Server=(localdb)\mssqllocaldb;Database=Lesson6.Products;Trusted_Connection=True;";
            services.AddDbContext<WebshopRepository>(options => options.UseSqlServer(connection));
            
            services.AddTransient<IDateTime,SystemDateTime>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            //services.AddSingleton<IDateTime,SystemDateTime>();
            

            services.AddMvc().AddDataAnnotationsLocalization().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] {

                    new CultureInfo("sv"),
                    new CultureInfo("en")

                };

                options.DefaultRequestCulture = new RequestCulture(culture: "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

            });

            

            //localisationOptions.RequestCultureProviders.Insert(0, new UrlCultureProvider());

            //app.UseRequestLocalization(localisationOptions);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
           

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            //   app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //   app.UseApplicationInsightsExceptionTelemetry();

          
            

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //   name: "culture",
                //   template: "{language:regex(^[a-z]{{2}}(-[A-Z]{{2}})*$)}/{controller=Products}/{action=Index}/{searchString?}"

                //    );

                //routes.MapRoute(
                //   name: "default",
                //   template: "{controller=Products}/{action=Index}/{searchString?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Products}/{action=Index}/{id?}");


                //routes.MapRoute(
                //   name: "language",
                //   template: "{lang=se}/{controller=Home}/{action=About}/");


                //routes.MapRoute(
                //    name: "twoparameters",
                //    template: "{controller}/{action}/{id}/{name}");


                //routes.MapRoute(
                //    name: "longroute",
                //    template: "Sverige/{controller}/{action}/{id}/{name}");






            });
        }
    }
}
