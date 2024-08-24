namespace Mango.Services.OrderApi.Models.Dtos
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; } = default!;
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
    }
}
