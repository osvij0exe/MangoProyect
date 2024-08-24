using AutoMapper;
using Mango.Services.ProductAPI.Entities;
using Mango.Services.ProductAPI.Model.Request;
using Mango.Services.ProductAPI.Model.Response;

namespace Mango.Services.ProductAPI.Profiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<Product, ProductResponseDto>().ReverseMap();
            CreateMap<ProductRequestDto, Product>().ReverseMap();
        }
    }
}
