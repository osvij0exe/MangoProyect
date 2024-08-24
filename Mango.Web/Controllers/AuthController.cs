using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _service;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService service, ITokenProvider tokenProvider)
        {
            _service = service;
            _tokenProvider = tokenProvider;
        }


        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto requestDto)
        {

            ResponseDto responseDto = await _service.LoginAsync(requestDto);

            if (responseDto != null && responseDto.IsSucess)
            {
                LoginResponseDto loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));

                await SignInUser(loginResponse);
                _tokenProvider.SetToken(loginResponse.Token);


                return RedirectToAction("Index", "Home");

            }
            else
            {
                //ModelState.AddModelError("CustomError", responseDto.Message);
                TempData["error"] = responseDto.Message;
                return View(requestDto);
            }
        }



        [HttpGet]
        public IActionResult Register()
        {
            var rolelist = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
                new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer}
            };

            ViewBag.RoleList = rolelist;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto requestDto)
        {

            ResponseDto result = await _service.RegisterAsync(requestDto);

            ResponseDto assignRole;
            
            if(result != null && result.IsSucess)
            {
                if(string.IsNullOrEmpty(requestDto.Role))
                {
                    requestDto.Role = SD.RoleCustomer;
                }
                assignRole = await _service.AssingRoleAsync(requestDto);
                if(assignRole != null && assignRole.IsSucess)
                {
                    TempData["success"] = "Registration succesful";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = result.Message;
                
            }
            var rolelist = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
                new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer}
            };

            ViewBag.RoleList = rolelist;


            return View(requestDto);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index","Home");
        }

        private async Task SignInUser(LoginResponseDto model)
        {


            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            
            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(ClaimTypes.Role,
              jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));


            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);
        }

    }
}
