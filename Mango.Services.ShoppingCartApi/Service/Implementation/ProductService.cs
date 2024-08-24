using Mango.Services.ShoppingCartApi.Model;
using Mango.Services.ShoppingCartApi.Model.Dto;
using Mango.Services.ShoppingCartApi.Service.Iservice;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartApi.Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProductService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _clientFactory.CreateClient("Product");
            var resposne = await client.GetAsync($"/api/ProductApi");
            var apiContent = await resposne.Content.ReadAsStringAsync();

            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if(resp.IsSucess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Result));
            }
            return new List<ProductDto>();

        }
    }
}
