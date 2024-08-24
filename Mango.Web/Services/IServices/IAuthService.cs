using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IAuthService
    {

        Task<ResponseDto?> LoginAsync(LoginRequestDto requestDto);
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDto requestDto);
        Task<ResponseDto?> AssingRoleAsync(RegistrationRequestDto requestDto);
    }
}
