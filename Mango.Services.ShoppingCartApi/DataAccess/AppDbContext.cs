using Mango.Services.ShoppingCartApi.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Mango.Services.ShoppingCartApi.DataAccess
{
    public class AppDbContext :DbContext
    {
        public AppDbContext( DbContextOptions<AppDbContext> options)
            :base(options) 
        {       
            
        }

        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }


    }
}
