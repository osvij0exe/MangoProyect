using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IshoppingCartService _cartService;
        private readonly IOrderService _orderService;

        public CartController(IshoppingCartService cartService,
            IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }


        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _cartService.RemoveFromCartAsync(cartDetailsId);

            if (response is not null && response.IsSucess)
            {
                TempData["success"] = "Cart updated susscessfully";
                return RedirectToAction(nameof(CarIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {

            ResponseDto? response = await _cartService.ApplyCouponAsync(cartDto);

            if (response is not null && response.IsSucess)
            {
                TempData["success"] = "Cart updated susscessfully";
                return RedirectToAction(nameof(CarIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            CartDto cart = await LoadCarDtoBaseOnLoggedInUser();
            cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;


            ResponseDto? response = await _cartService.EmailCartAsync(cart);

            if (response is not null && response.IsSucess)
            {
                TempData["success"] = "Email will be processed and sent shortly. ";
                return RedirectToAction(nameof(CarIndex));
            }
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = "";
            ResponseDto? response = await _cartService.ApplyCouponAsync(cartDto);

            if (response is not null && response.IsSucess)
            {
                TempData["success"] = "Cart updated susscessfully";
                return RedirectToAction(nameof(CarIndex));
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> CheckOut()
        {

            return View(await LoadCarDtoBaseOnLoggedInUser());
        }

        [HttpPost]
        [ActionName("CheckOut")]
        public async Task<IActionResult> CheckOut(CartDto cartDto)
        {

            CartDto cart = await LoadCarDtoBaseOnLoggedInUser();

            cart.CartHeader.Phone = cartDto.CartHeader.Phone;
            cart.CartHeader.Email = cartDto.CartHeader.Email;
            cart.CartHeader.Name = cartDto.CartHeader.Name;

            var response = await _orderService.CreateOrderAsync(cart);
            OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

            if(orderHeaderDto is not null && response.IsSucess)
            {
                // get stripe session and redirecrt to stripe to place order
                //se requiere crear una cuenta en la plataforma de stripe

                //variable domain
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";


                StripeRequestDto stripeRequestDto = new()
                {
                    ApproveUrl = domain + "Cart/Confirmation?orderId=" + orderHeaderDto.OrderHeaderId,
                    CanelUrl = domain + "Cart/CheckOut",
                    OrderHeader = orderHeaderDto
                };

                var stripeResponse = await _orderService.CreateStripeSessionAsync(stripeRequestDto);

                StripeRequestDto stripeResponseResult = JsonConvert.DeserializeObject<StripeRequestDto>(Convert.ToString(stripeResponse.Result));
                Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);

                return new StatusCodeResult(303);

            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Confirmation(int orderId)
        {

            ResponseDto? response = await _orderService.ValidateStripeSessionAsync(orderId);

            if (response is not null && response.IsSucess)
            {
                OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
                if(orderHeaderDto.Status == SD.Status_Approved)
                {
                    return View(orderId);

                }
            }
                //redirect to some error page based on status
                return View(orderId);
        }


        [Authorize]
        public async Task<IActionResult> CarIndex()
        {



            return View( await LoadCarDtoBaseOnLoggedInUser());
        }

        private async Task<CartDto> LoadCarDtoBaseOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _cartService.GetCartByUserIdAsync(userId);

            if (response is not null && response.IsSucess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return cartDto;
            }
            return new CartDto();
        }




    }
}
