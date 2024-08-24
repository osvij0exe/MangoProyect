using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;

namespace Mango.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseServices _baseServices;

        public AuthService(IBaseServices baseServices)
        {
            _baseServices = baseServices;
        }

        public async Task<ResponseDto?> AssingRoleAsync(RegistrationRequestDto requestDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = requestDto,
                Url = SD.AuthApiBase + "/api/AuthAPI/AssingRole"
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto requestDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = requestDto,
                Url = SD.AuthApiBase + "/api/AuthAPI/Login"
            }, withBearer: false);
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto requestDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = requestDto,
                Url = SD.AuthApiBase + "/api/AuthAPI/register"
            }, withBearer: false);
        }
    }
}
