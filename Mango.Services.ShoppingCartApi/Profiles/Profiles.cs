using AutoMapper;
using Mango.Services.ShoppingCartApi.Model;
using Mango.Services.ShoppingCartApi.Model.Dto;

namespace Mango.Services.ShoppingCartApi.Profiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
            CreateMap<CartDetails,CartDetailsDto>().ReverseMap();
        }
    }
}
