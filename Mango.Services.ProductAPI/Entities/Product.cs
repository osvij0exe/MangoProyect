using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
        public double Price { get; set; }
        public string Description { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
        public string? ImageUrl { get; set; } = default!;
        public string? IamgeLocalPath { get; set; } = default!;
    }
}
