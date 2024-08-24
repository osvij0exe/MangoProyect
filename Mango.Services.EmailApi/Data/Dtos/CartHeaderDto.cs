namespace Mango.Services.EmailApi.Data.Dtos
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }

        public double CartTotal { get; set; }

        //se requiee una suscrpcion de AZURE y generear un BusService
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
