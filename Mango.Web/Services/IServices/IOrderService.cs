using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IOrderService
    {
        Task<ResponseDto?> CreateOrderAsync(CartDto cartDto);
        Task<ResponseDto?> CreateStripeSessionAsync(StripeRequestDto stripeRequestDto);
        Task<ResponseDto?> ValidateStripeSessionAsync(int orderHeaderId);
        Task<ResponseDto?> GetAllOrder(string? userId);
        Task<ResponseDto?> GetOrder(int orderId);
        Task<ResponseDto?> UpdateOrderStatus(int orderId, string newStatus);

    }
}
