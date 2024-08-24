using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;

namespace Mango.Web.Services
{
    public class ShoppingCartService : IshoppingCartService
    {
        private readonly IBaseServices _baseServices;

        public ShoppingCartService(IBaseServices baseServices)
        {
            _baseServices = baseServices;
        }

        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartApiBase + "/api/ShoppingCarApi/ApplyCoupon"
            });
        }


        //NOTA: Se requiere de un servicio de AZURE y un serviceBus Creado
        public async Task<ResponseDto?> EmailCartAsync(CartDto cartDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartApiBase + "/api/ShoppingCarApi/EmailCartRequest"
            });
        }

        public async Task<ResponseDto?> GetCartByUserIdAsync(string userId)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ShoppingCartApiBase + "/api/ShoppingCarApi/GetCart/" + userId
            });
        }

        public async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDetailsId,
                Url = SD.ShoppingCartApiBase + "/api/ShoppingCarApi/RemoveCart"
            });
        }

        public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data= cartDto,
                Url=SD.ShoppingCartApiBase + "/api/ShoppingCarApi/CartUpsert"
            });
        }
    }
}
