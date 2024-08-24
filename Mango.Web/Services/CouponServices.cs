using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;

namespace Mango.Web.Services
{
    public class CouponServices : ICouponServices
    {
        private readonly IBaseServices _baseServices;

        public CouponServices(IBaseServices baseServices)
        {
            _baseServices = baseServices;
        }

        public async Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = couponDto,
                Url = SD.CouponApiBase + "/api/CouponApi"
			});
        }


        public async Task<ResponseDto?> GetAllCouponAsync()
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponApiBase + "/api/CouponApi"
			});
        }

        public async Task<ResponseDto?> GetCouponByCodeAsync(string couponCode)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponApiBase + "/api/CouponApi/GetByCode/" + couponCode
            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
                       return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponApiBase + "/api/CouponApi/" + id
            });
        }

        public async Task<ResponseDto?> DeleteCoupon(int id)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.CouponApiBase + "/api/CouponApi/" + id
            });
        }

        public async Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = couponDto,
                Url = SD.CouponApiBase + "/api/CouponApi"
			});
        }
    }
}
