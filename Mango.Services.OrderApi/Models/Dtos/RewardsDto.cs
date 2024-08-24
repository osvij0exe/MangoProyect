namespace Mango.Services.OrderApi.Models.Dtos
{
    public class RewardsDto
    {
        public string UserId { get; set; } = default!;
        public int RewardsActivity { get; set; }
        public int OrderId { get; set; }
    }
}
