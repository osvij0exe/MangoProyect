namespace Mango.Services.ShoppingCartApi.Model.Dto
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; } = default!;
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }

    }
}
