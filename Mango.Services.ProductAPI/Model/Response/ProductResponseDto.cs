namespace Mango.Services.ProductAPI.Model.Response
{
    public class ProductResponseDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
        public double Price { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
        public string? ImageUrl { get; set; } = default!;
        public string? IamgeLocalPath { get; set; } = default!;
        public IFormFile? Image { get; set; }

    }
}
