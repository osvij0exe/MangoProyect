namespace Mango.Web.Models
{
    public class OrderDetailsDto
    {
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }
        public int ProductId { get; set; }
        public ProductDto? ProductDto { get; set; } = default!;
        public int Count { get; set; }
        public string ProdutName { get; set; } = default!;
        public double Price { get; set; }
    }
}
