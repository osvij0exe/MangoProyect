﻿using System.ComponentModel.DataAnnotations;

namespace Mango.Services.OrderApi.Models
{
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }

        public double OrderTotal { get; set; }

        //se requiee una suscrpcion de AZURE y generear un BusService
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime OrderTime { get; set; }
        public string? Status { get; set; }



        public string? PaymentIntenrId { get; set; }
        public string? StripeSessionId { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }


    }
}
