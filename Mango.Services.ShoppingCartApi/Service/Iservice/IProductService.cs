using Mango.Services.ShoppingCartApi.Model;

namespace Mango.Services.ShoppingCartApi.Service.Iservice
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts(); 

    }
}
