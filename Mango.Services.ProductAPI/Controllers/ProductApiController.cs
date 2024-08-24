using AutoMapper;
using Mango.Services.ProductAPI.DataAcces;
using Mango.Services.ProductAPI.Entities;
using Mango.Services.ProductAPI.Model.Request;
using Mango.Services.ProductAPI.Model.Response;
using Mango.Services.ProductAPI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        /*
//private readonly IProductServices _services;

//public ProductApiController(IProductServices services)
//{
//    _services = services;
//}

//[HttpGet]
//public async Task<IActionResult> ProductList()
//{
//    var response = await _services.ProductListAsync();

//    return Ok(response);
//}

//[HttpGet("{id:int}")]
//public async Task<IActionResult> FindProductById(int id)
//{
//    var response = await _services.FindPorductByIdAsync(id);

//    return response.Success ? Ok(response) : NotFound(response);

//}

//[HttpPost]
//public async Task<IActionResult> CreateProduct(ProductRequestDto requestDto)

//{
//    var response = await _services.CreateProductAsync(requestDto);

//    return response.Success ? Ok(response) : BadRequest(response);  
//}

//[HttpPut]
//public async Task<IActionResult> UpdateProduct(int id, ProductRequestDto requestDto)
//{
//    var response = await _services.UpdateProductAsync(id, requestDto);

//    return response.Success ? Ok(response) : BadRequest(response);
//}

//[HttpDelete]
//public async Task<IActionResult> DeleteProduct(int id)
//{
//    var response = await _services.DeleteProductAsync(id);
//    return response.Success ? Ok(response) : NotFound(id);
//}
*/

        public ProductApiController(AppDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Product> objList = _context.Set<Product>().ToList();
                _mapper.Map<IEnumerable<Product>>(objList);
                _response.Result = objList;
                _response.IsSucess = true;



            }
            catch (Exception ex)
            {

                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ResponseDto Post(ProductRequestDto requestDto)
        {
            try
            {
                Product prodcut = _mapper.Map<Product>(requestDto);
                _context.Set<Product>().Add(prodcut);
                _context.SaveChanges();

                //almacenando la imagen en el wwwroot
                if(requestDto.Image != null)
                {
                    string fileName = prodcut.ProductId + Path.GetExtension(requestDto.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    using (var fileStream = new FileStream(filePathDirectory,FileMode.Create))
                    {
                        requestDto.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    prodcut.ImageUrl =baseUrl + "/ProductImages/"+ fileName;
                    prodcut.IamgeLocalPath = filePath;
                }
                else
                {
                    prodcut.ImageUrl = "https://placehold.co/600x400";
                }
                _context.Set<Product>().Update(prodcut);
                _context.SaveChanges();


                _response.IsSucess = true;
                _response.Result = _mapper.Map<ProductRequestDto>(prodcut);

                //if (prodcut is null)
                //{
                //    _response.IsSucess = false;
                //}

            }
            catch (Exception ex)
            {

                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPut]
        //fala generaar la vista de edit en el frontend
        public ResponseDto Put([FromBody] ProductRequestDto requestDto)
        {
            try
            {

                Product product = _mapper.Map<Product>(requestDto);
                
                
                if (requestDto.Image != null)
                {

                    if (!string.IsNullOrEmpty(product.IamgeLocalPath))
                    {
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.IamgeLocalPath);
                        FileInfo file = new FileInfo(oldFilePathDirectory);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    string fileName = product.ProductId + Path.GetExtension(requestDto.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        requestDto.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                    product.IamgeLocalPath = filePath;
                }

                _context.Set<Product>().Update(product);
                _context.SaveChanges();
                _response.Result = _mapper.Map<ProductRequestDto>(product);
                _response.IsSucess = true;



            }
            catch (Exception ex)
            {

                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;

            }

        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDto Delete(int id)
        {

            try
            {

                Product obj = _context.Set<Product>().First(u => u.ProductId == id);

                if(!string.IsNullOrEmpty(obj.IamgeLocalPath))
                {
                    var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), obj.IamgeLocalPath);
                    FileInfo file = new FileInfo(oldFilePathDirectory);
                    if(file.Exists)
                    {
                        file.Delete();
                    }
                }


                _context.Set<Product>().Remove(obj);
                _context.SaveChanges();
                _response.IsSucess = true;



            }
            catch (Exception ex)
            {

                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto GetById(int id)
        {

            try
            {

                Product obj = _context.Set<Product>().First(u => u.ProductId == id);
                _mapper.Map<ProductResponseDto>(obj);
                _response.Result = obj;
                _response.IsSucess = true;


            }
            catch (Exception ex)
            {

                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;

        }

    }
}
