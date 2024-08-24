using Mango.Services.OrderApi.Models.Dtos;

namespace Mango.Services.OrderApi.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();

    }
}
