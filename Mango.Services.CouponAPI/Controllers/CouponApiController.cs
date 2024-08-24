using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CouponApiController :ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public CouponApiController(AppDbContext db,IMapper mapper )
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {

                IEnumerable<Coupon> objList = _db.Coupons.ToList();
                _mapper.Map<IEnumerable<CouponDto>>(objList);
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

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {

                Coupon obj = _db.Coupons.First(u => u.CouponCode.ToLower() == code.ToLower())!;

                if(obj is null)
                {
                    _response.IsSucess = false;
                }


                _mapper.Map<CouponDto>(obj);
                _response.Result = obj;

            }
            catch (Exception ex)
            {

                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] CouponDto coupondRequest)
        {
            try
            {

                Coupon obj = _mapper.Map<Coupon>(coupondRequest);
                _db.Coupons.Add(obj);
                _db.SaveChanges();



                var options = new Stripe.CouponCreateOptions
                {
                    Duration = "repeating",
                    Name=coupondRequest.CouponCode,
                    Currency ="usd",
                    Id=coupondRequest.CouponCode,
                    AmountOff = (long)(coupondRequest.DiscountAmount*100),
                };
                var service = new Stripe.CouponService();
                service.Create(options);



                _response.IsSucess = true;
                _response.Result = _mapper.Map<CouponDto>(obj);

                if (obj is null)
                {
                    _response.IsSucess = false;
                }


                _mapper.Map<CouponDto>(obj);
                _response.Result = obj;

            }
            catch (Exception ex)
            {

                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {

                Coupon obj = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Update(obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<CouponDto>(obj);
                _response.IsSucess = true;

            }
            catch (Exception ex)
            {

                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {

                Coupon obj = _db.Coupons.First(u => u.CouponId == id);
                _db.Coupons.Remove(obj);
                _db.SaveChanges();
                _response.IsSucess = true;

         
                var service = new Stripe.CouponService();
                service.Delete(obj.CouponCode);





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

                Coupon obj = _db.Coupons.First(u => u.CouponId == id);
                _mapper.Map<CouponDto>(obj);
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
