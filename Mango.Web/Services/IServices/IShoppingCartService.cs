using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IshoppingCartService
    {
        Task<ResponseDto?> GetCartByUserIdAsync(string userId);
        Task<ResponseDto?> UpsertCartAsync(CartDto cartDto);
        Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId);
        Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto);

        //Nota: Se requeire de una suscripcion de AZURE y un serviceBus Creado
        Task<ResponseDto?> EmailCartAsync(CartDto cartDto);

    }
}
