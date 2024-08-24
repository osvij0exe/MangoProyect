using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;

namespace Mango.Services.CouponAPI.Profiles
{
    public class Profiles : Profile
    {

        public Profiles()
        {
            CreateMap<Coupon, CouponDto>().ReverseMap();
        }

    }
}
