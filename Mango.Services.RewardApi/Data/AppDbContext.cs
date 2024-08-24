﻿using Mango.Services.RewardApi.Model;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.RewardApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options) 
        { 
        }


        public DbSet<Rewards> Rewards { get; set; }


    }
}
