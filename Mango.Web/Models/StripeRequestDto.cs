namespace Mango.Web.Models
{
    public class StripeRequestDto
    {
        // se requiere suscripcion a stripe
        public string? StripeSessionUrl { get; set; } 
        public string? StripeSessionId { get; set; } 

        public string ApproveUrl { get; set; } = default!;
        public string CanelUrl { get; set; } = default!;
        public OrderHeaderDto OrderHeader { get; set; } = default!;

    }
}
