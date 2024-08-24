using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Model.Request
{
    public class ProductRequestDto
    {
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public double Price { get; set; }
        public string? Description { get; set; } = default!;
        public string? CategoryName { get; set; } = default!;
        public string? ImageUrl { get; set; } = default!;
        public string? IamgeLocalPath { get; set; } = default!;
        public IFormFile? Image { get; set; }

    }
}
