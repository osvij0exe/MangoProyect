using Mango.Services.EmailApi.Data.Dtos;
using Mango.Services.EmailApi.Message;

namespace Mango.Services.EmailApi.Services
{
    public interface IEmailService
    {

        Task EmailCartAndLog(CartDto cartDto);

        Task RegisterUserEmailAndLog(string email);

        Task LogOrderPlaced(RewardsMessage rewardsDto);


    }
}
