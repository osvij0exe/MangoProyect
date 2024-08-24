using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;

namespace Mango.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseServices _baseServices;

        public ProductService(IBaseServices baseServices)
        {
            _baseServices = baseServices;
        }



        public async Task<ResponseDto?> CreateProductAsync(ProductDto productDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = productDto,
                Url = SD.ProductApiBase + "/api/ProductApi",
                ContentType = SD.ContentType.MultipartFormData
			});
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ProductApiBase + "/api/ProductApi/" + id
            });
        }

        public async Task<ResponseDto?> GetAllProductAsync()
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductApiBase + "/api/ProductApi"
			});
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductApiBase + "/api/ProductApi/" + id
            });
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto productDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = productDto,
                Url = SD.ProductApiBase + "/api/ProductApi",
                ContentType = SD.ContentType.MultipartFormData
			});
        }
    }
}
