using Mango.Services.RewardApi.Data;
using Mango.Services.RewardApi.Message;
using Mango.Services.RewardApi.Model;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.RewardApi.Services
{
    public class RewardsServices : IRewardsService
    {
        private DbContextOptions<AppDbContext> _contextOptions;

        public RewardsServices(DbContextOptions<AppDbContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }
        public async Task UpdateRewards(RewardsMessage rewardsMessage)
        {
            try
            {
                Rewards rewards = new Rewards()
                {
                    OrderId = rewardsMessage.OrderId,
                    RewardsActivity = rewardsMessage.RewardsActiviry,
                    UserId = rewardsMessage.UserId,
                    RewardsDate = DateTime.Now,
                };

                await using var _context = new AppDbContext(_contextOptions);
                await _context.Rewards.AddAsync(rewards);
                await _context.SaveChangesAsync();


            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
