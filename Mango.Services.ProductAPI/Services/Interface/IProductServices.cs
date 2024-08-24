using Mango.Services.ProductAPI.Model.Request;
using Mango.Services.ProductAPI.Model.Response;

namespace Mango.Services.ProductAPI.Services.Interface
{
    public interface IProductServices
    {
        Task<CollectionBaseRepsone<ProductResponseDto>> ProductListAsync();
        Task<BaseResponseGeneric<ProductResponseDto>> FindPorductByIdAsync(int id);
        Task<BaseReponse> CreateProductAsync(ProductRequestDto requestDto);
        Task<BaseReponse> UpdateProductAsync(int id, ProductRequestDto requestDto);
        Task<BaseReponse> DeleteProductAsync(int id);
 
    }
}
