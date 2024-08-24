namespace Mango.Services.OrderApi.Models.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
        public double Price { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
    }
}
