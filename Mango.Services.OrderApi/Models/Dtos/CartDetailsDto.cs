namespace Mango.Services.OrderApi.Models.Dtos
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public CartHeaderDto? CartHeader { get; set; } = default!;
        public int ProductId { get; set; }
        public ProductDto? ProductDto { get; set; } = default!;
        public int Count { get; set; }
    }
}
