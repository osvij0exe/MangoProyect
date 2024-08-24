﻿using Mango.Services.OrderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.OrderApi.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {


        }

        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
