using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController: Controller
    {
        private readonly ICouponServices _couponServices;

        public CouponController(ICouponServices couponServices)
        {
            _couponServices = couponServices;
        }

        public async Task<ActionResult> CouponIndex()
        {

            List<CouponDto>? couponList = new();

            ResponseDto? response  =  await _couponServices.GetAllCouponAsync();

            if(response is not null && response.IsSucess)
            {
                couponList = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(couponList);
        }


        public async Task<IActionResult> CreateCoupon()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CouponDto model)
        {
            if(ModelState.IsValid)
            {
                ResponseDto? response = await _couponServices.CreateCouponAsync(model);
                
                if(response != null && response.IsSucess)
                {
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }

            }



            return View(model);
        }


        public async Task<IActionResult> CouponDelete(int couponId)
        {

            ResponseDto? response = await _couponServices.GetCouponByIdAsync(couponId);
            
            if(response != null && response.IsSucess)
            {
                CouponDto? model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
                return View(model);
            }


            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
            ResponseDto? response = await _couponServices.DeleteCoupon(couponDto.CouponId);

            if(response != null && response.IsSucess)
            {
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }



            return View(couponDto);

        }

    }
}
