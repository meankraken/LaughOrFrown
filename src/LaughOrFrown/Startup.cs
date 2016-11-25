using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using LaughOrFrown.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LaughOrFrown
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder().SetBasePath(_env.ContentRootPath).AddJsonFile("config.json").AddEnvironmentVariables();
            _config = builder.Build();
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_config);

            services.AddIdentity<LaughUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Cookies.ApplicationCookie.LoginPath = "/";
            })
            .AddEntityFrameworkStores<LaughContext>();

            services.AddDbContext<LaughContext>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(config =>
            {
                config.MapRoute("Default", "{controller}/{action}/{id?}", new { controller = "App", action = "Index" });
                config.MapRoute("Jokes", "{action}/{id?}", new { controller = "App" });
            });
        }
    }
}
