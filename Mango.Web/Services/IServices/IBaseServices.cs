using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IBaseServices
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true);

    }
}
