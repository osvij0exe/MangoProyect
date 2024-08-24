namespace Mango.Services.RewardApi.Message
{
    public class RewardsMessage
    {
        public string UserId { get; set; } = default!;
        public int RewardsActiviry { get; set; }
        public int OrderId { get; set; }
    }
}
