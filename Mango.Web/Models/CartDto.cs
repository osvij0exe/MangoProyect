namespace Mango.Web.Models
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; } = default!;
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
    }
}
