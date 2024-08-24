using Mango.Services.OrderApi.Models.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.OrderApi.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public OrderHeader? OrderHeader { get; set; } = default!;
        public int ProductId { get; set; }
        [NotMapped]
        public ProductDto? ProductDto { get; set; } = default!;
        public int Count { get; set; }
        public string ProdutName { get; set; } = default!;
        public double Price { get; set; } 
    }
}
