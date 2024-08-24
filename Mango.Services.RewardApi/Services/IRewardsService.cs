using Mango.Services.RewardApi.Message;

namespace Mango.Services.RewardApi.Services
{
    public interface IRewardsService
    {
        Task UpdateRewards(RewardsMessage rewardsMessage);
    }
}
