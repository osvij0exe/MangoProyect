using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Mango.Services.ProductAPI.Entities.Configuration
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.ProductId)
                .IsRequired();
            builder.Property(p => p.Name)
                .IsRequired();
            builder.Property(p => p.Price)
                .IsRequired();
            builder.Property(p => p.Description)
                .HasMaxLength(1000);
            builder.Property(p => p.CategoryName)
                .HasMaxLength(1000);
            builder.Property(p => p.ImageUrl)
            .HasMaxLength (1000);

            builder.HasData( new List<Product>
            {
                new()
                {                 
                    ProductId = 1,
                    Name = "Samosa",
                    Price = 15,
                    Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                    ImageUrl = "https://placehold.co/603x403",
                    CategoryName = "Appetizer"
                },
                new() 
                {
                    ProductId = 2,
                    Name = "Paneer Tikka",
                    Price = 13.99,
                    Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                    ImageUrl = "https://placehold.co/602x402",
                    CategoryName = "Appetizer"
                },
                new()
                {
                    ProductId = 3,
                    Name = "Sweet Pie",
                    Price = 10.99,
                    Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                    ImageUrl = "https://placehold.co/601x401",
                    CategoryName = "Dessert"
                },
                new()
                {
                    ProductId = 4,
                    Name = "Pav Bhaji",
                    Price = 15,
                    Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                    ImageUrl = "https://placehold.co/600x400",
                    CategoryName = "Entree"
                }
            });



        }
    }
}
