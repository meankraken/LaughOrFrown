﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LaughOrFrown.Models
{
    public class LaughContext : IdentityDbContext<LaughUser> 
    {
        public IConfigurationRoot _config;

        public LaughContext(IConfigurationRoot config, DbContextOptions options) : base(options)
        {
            _config = config;
        }

        public DbSet<Joke> Jokes { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            //optionsBuilder.UseSqlServer(_config["ConnectionStrings:LaughContextConnection"]);
            optionsBuilder.UseSqlServer(_config["ConnectionStrings:DeployConnection"]);
        }
    }
}
