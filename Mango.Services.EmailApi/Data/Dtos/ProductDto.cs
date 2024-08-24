using System.ComponentModel.DataAnnotations;

namespace Mango.Services.EmailApi.Data.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
        public double Price { get; set; }
        public string Description { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;

        [Range(1, 100)]
        public int Count { get; set; } = 1;
    }
}
