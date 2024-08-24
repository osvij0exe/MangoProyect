using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;

namespace Mango.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseServices _baseServices;

        public OrderService(IBaseServices baseServices)
        {
            _baseServices = baseServices;
        }

        public async Task<ResponseDto?> CreateOrderAsync(CartDto cartDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.OrdeApiBase + "api/OrderApi/CreateOrder"
            });
        }

        public async Task<ResponseDto?> CreateStripeSessionAsync(StripeRequestDto stripeRequestDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = stripeRequestDto,
                Url = SD.OrdeApiBase + "api/OrderApi/CreateStripeSession"
            });
        }

        public async Task<ResponseDto?> GetAllOrder(string? userId)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.OrdeApiBase + "api/OrderApi/GetOrders/GetOrders/" + userId
            });
        }

        public async Task<ResponseDto?> GetOrder(int orderId)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.OrdeApiBase + "api/OrderApi/GetOrder/" + orderId
            });
        }

        public async Task<ResponseDto?> UpdateOrderStatus(int orderId, string newStatus)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = newStatus,
                Url = SD.OrdeApiBase + "api/OrderApi/UpdateOrderStatus/" + orderId
            });
        }

        public async Task<ResponseDto?> ValidateStripeSessionAsync(int orderHeaderId)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = orderHeaderId,
                Url = SD.OrdeApiBase + "api/OrderApi/ValidateStripeSession"
            });
        }
    }
}
