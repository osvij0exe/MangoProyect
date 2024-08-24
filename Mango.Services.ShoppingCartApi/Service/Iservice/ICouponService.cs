using Mango.Services.ShoppingCartApi.Model;
using Mango.Services.ShoppingCartApi.Model.Dto;

namespace Mango.Services.ShoppingCartApi.Service.Iservice
{
    public interface ICouponService
    {

        Task<CouponDto> GetCoupon(string couponCode);
    }
}
