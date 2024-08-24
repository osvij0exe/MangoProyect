namespace Mango.Services.EmailApi.Data.Dtos
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; } = default!;
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
    }
}
