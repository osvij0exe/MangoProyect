using AutoMapper;
using Mango.Services.ProductAPI.DataAcces;
using Mango.Services.ProductAPI.Entities;
using Mango.Services.ProductAPI.Model.Request;
using Mango.Services.ProductAPI.Model.Response;
using Mango.Services.ProductAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Mango.Services.ProductAPI.Services.Implementation
{
    public class ProductServices : IProductServices
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductServices> _logger;
        private readonly IMapper _mapper;

        public ProductServices(AppDbContext context,
            ILogger<ProductServices> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<CollectionBaseRepsone<ProductResponseDto>> ProductListAsync()
        {
            var resposne= new CollectionBaseRepsone<ProductResponseDto>();

            try
            {

                var listProducts = _context.Set<Product>()
                    .AsQueryable();

                var collection = await listProducts
                    .OrderBy(p => p.CategoryName)
                    .Select(p => new ProductResponseDto()
                    {
                        Name = p.Name,
                        Price = p.Price,
                        Description = p.Description,
                        CategoryName = p.CategoryName,
                        ImageUrl = p.ImageUrl
                    }).ToListAsync();

                resposne.Data = collection;
                resposne.Success = true;

            }
            catch (Exception ex)
            {

                resposne.ErrorMessage = "Error al listar producto";
                _logger.LogCritical(ex, "{ErrorMessage}{Message}", resposne.ErrorMessage, ex.Message);
            }

            return resposne;
        }
        public async Task<BaseResponseGeneric<ProductResponseDto>> FindPorductByIdAsync(int id)
        {
            var response = new BaseResponseGeneric<ProductResponseDto>();


            try
            {

                var product = await _context.Set<Product>().FirstOrDefaultAsync(p => p.ProductId == id);


                if (product is not null)
                {
                    response.Data = _mapper.Map<ProductResponseDto>(product);
                    response.Success = true;
                }
                else
                {
                    throw new InvalidOperationException($"no se encontro ningun producto con el id {id}");
                }
            
            }
            catch(InvalidOperationException ex)
            {
                response.ErrorMessage = ex.Message;
                _logger.LogWarning(ex, "{ErrorMessage}{Message}", response.ErrorMessage, ex.Message);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al enctonrar el Producto por Id";

                _logger.LogCritical(ex, "{ErrorrMessage}{Message}", response.ErrorMessage, ex.Message);
                
            }
            return response;

        }



        public async Task<BaseReponse> CreateProductAsync(ProductRequestDto requestDto)
        {
            var response = new BaseReponse();

            try
            {

                var Product = _mapper.Map<Product>(requestDto);

                var newProduct = await _context.AddAsync(Product);
                await _context.SaveChangesAsync();
                response.Success = true;


            }
            catch (Exception ex)
            {

                response.ErrorMessage = "Error al crear producto";
                _logger.LogCritical(ex, "{ErrorMessage}{Message}", response.ErrorMessage, ex.Message);
            }

            return response;
        }
        public async Task<BaseReponse> UpdateProductAsync(int id, ProductRequestDto requestDto)
        {
            var response = new BaseReponse();

            try
            {

                var registro = await _context.Set<Product>().FirstOrDefaultAsync(p => p.ProductId == id);


                if(registro is not null)
                {
                    var mapProduct = _mapper.Map( requestDto,registro);
                    await _context.SaveChangesAsync();
                    response.Success = registro !=  null;
                }
                else
                {
                    throw new InvalidOperationException($"No se encontro ningun registro cone l id: {id}");
                }


            }
            catch(InvalidOperationException ex)
            {
                response.ErrorMessage = ex.Message;
                _logger.LogWarning(ex, "{ErrorMessage}{Message}",response.ErrorMessage,ex.Message);
            }
            catch (Exception ex)
            {

                response.ErrorMessage = "Error al actualizar el producto";
                _logger.LogCritical(ex, "{ErrorMessage}{Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseReponse> DeleteProductAsync(int id)
        {
            var response = new BaseReponse();

            try
            {
                var registro = await _context.Set<Product>().FirstOrDefaultAsync(p => p.ProductId == id);

                if (registro is not null)
                {
                    _context.Remove(registro);
                    await _context.SaveChangesAsync();
                    response.Success = true;
                }
                else
                {
                    throw new InvalidOperationException($"no se encontro ningun producto con el id {id}");
                }

            }
            catch (InvalidOperationException ex)
            {
                response.ErrorMessage = ex.Message;
                _logger.LogWarning(ex, "{ErrorMessage}{Message}", response.ErrorMessage, ex.Message);
            }
            catch (Exception ex)
            {

                response.ErrorMessage = "Error al eliminar producto";
                _logger.LogCritical(ex, "{ErrorMessage}{Message}", response.ErrorMessage, ex.Message);
            }
            return response;

        }

    }
}
