using Mango.Services.ShoppingCartApi.Model;
using Mango.Services.ShoppingCartApi.Model.Dto;
using Mango.Services.ShoppingCartApi.Service.Iservice;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartApi.Service.Implementation
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _clientFactory;

        public CouponService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var client = _clientFactory.CreateClient("Coupon");
            var resposne = await client.GetAsync($"/api/CouponApi/GetByCode/{couponCode}");
            var apiContent = await resposne.Content.ReadAsStringAsync();

            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (resp != null &&  resp.IsSucess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
            }
            return new CouponDto();
        }
    }
}
