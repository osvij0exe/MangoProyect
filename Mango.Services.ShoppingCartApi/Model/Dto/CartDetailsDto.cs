using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartApi.Model.Dto
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
