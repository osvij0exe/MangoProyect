using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController :Controller
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }


        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? productList = new();

            ResponseDto? response = await _service.GetAllProductAsync();

            if(response is not null && response.IsSucess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productList);
        }

        
        public  ActionResult CreateProduct()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto model)
        {

            if(ModelState.IsValid)
            {
                ResponseDto? response = await _service.CreateProductAsync(model);

                if(response != null && response.IsSucess)
                {
                    return RedirectToAction(nameof(ProductIndex));

                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            ResponseDto? response = await _service.GetProductByIdAsync(id);
            
            if(response != null && response.IsSucess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(model);
            }

            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(ProductDto productDto)
        {

            ResponseDto? response = await _service.DeleteProductAsync(productDto.ProductId);

            if(response != null && response.IsSucess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }


            return View(productDto);

        }

    }
}
