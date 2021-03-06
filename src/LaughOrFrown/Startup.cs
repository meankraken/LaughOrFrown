﻿using System;
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
using AutoMapper;
using LaughOrFrown.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
                config.User.RequireUniqueEmail = false; //no emails needed for this application
                config.Password.RequiredLength = 8;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Cookies.ApplicationCookie.LoginPath = "/";
            })
            .AddEntityFrameworkStores<LaughContext>();

            services.AddDbContext<LaughContext>();

            
            services.AddTransient<LaughContextSeed>();
            services.AddScoped<ILaughRepository, LaughRepository>();
            
            services.AddLogging();
            
            services.AddMvc(options =>
            {
                //options.Filters.Add(new RequireHttpsAttribute()); 
            });
            
            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, LaughContextSeed seed)
        {
             
            loggerFactory.AddConsole();
            
            app.UseStaticFiles();

            app.UseIdentity();

            Mapper.Initialize(config =>
            {
                config.CreateMap<Joke, JokeViewModel>().ReverseMap();
                config.CreateMap<AddJokeViewModel, Joke>();
                config.CreateMap<RatingViewModel, Rating>();
                config.CreateMap<ProfileViewModel, LaughUser>().ReverseMap();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddDebug(LogLevel.Information);
            }
            else
            {
                app.UseStatusCodePages();
            }

            /*
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
            */

            app.UseMvc(config =>
            {
                config.MapRoute("Default", "{controller}/{action}/{id?}", new { controller = "App", action = "Index" });
                config.MapRoute("Jokes", "{action}/{id?}", new { controller = "App" });
            });
            
            seed.EnsureSeeded().Wait();
            
        }
    }
}
