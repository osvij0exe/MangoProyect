using Mango.Web.Utility;
using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
        public double Price { get; set; }
        public string Description { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
        public string? ImageUrl { get; set; } = default!;
        [MaxFileSize(1)]
        [AllowedExtensionsAtrribute(new string[] { ".jpg",".png"})]
        public string? IamgeLocalPath { get; set; } = default!;

        [Range(1,100)]
        public int Count { get; set; } = 1;
        public IFormFile? Image { get; set; }



    }
}
