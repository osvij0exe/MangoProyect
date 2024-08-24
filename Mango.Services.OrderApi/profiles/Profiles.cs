using AutoMapper;
using Mango.Services.OrderApi.Models;
using Mango.Services.OrderApi.Models.Dtos;

namespace Mango.Services.OrderApi.profiles
{
    public class Profiles :Profile
    {
        public Profiles()
        {
           CreateMap<OrderHeaderDto, CartHeaderDto>()
                .ForMember(destination => destination.CartTotal, u=>u.MapFrom(src => src.OrderTotal)).ReverseMap();

            CreateMap<CartDetailsDto, OrderDetailDto>()
                .ForMember(dest => dest.ProdutName, u => u.MapFrom(src => src.ProductDto.Name))
                .ForMember(dest => dest.Price, u => u.MapFrom(src => src.ProductDto.Price));


            CreateMap<OrderDetailDto, CartDetailsDto>().ReverseMap();
            CreateMap<OrderDetailDto, OrderDetails>().ReverseMap();
        }
    }
}
