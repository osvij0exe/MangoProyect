using Mango.MessageBus.Service.Interface;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        private readonly ResponsDto _response;

        public AuthAPIController(IAuthService service, 
            IMessageBus messageBus,
            IConfiguration configuration)
        {
            _service = service;
            _messageBus = messageBus;
            _configuration = configuration;
            _response = new();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto requestDto)
        {

            var errorMessage = await _service.Register(requestDto);
                
            if(!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSucess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            // se requiere Suscricion de Azure y crear un service Bus
            // await _messageBus.PublishMessage(requestDto.Email,_configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));
            return Ok(_response);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto) 
        {
            var loginResponse = await _service.Login(requestDto);

            if(loginResponse.User is null)
            {
                _response.IsSucess = false;
                _response.Message = "User or passord is incorrect";
                return BadRequest(_response);
            }

            _response.Result = loginResponse;
            return Ok(_response);
        }



        [HttpPost("AssingRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto requestDto)
        {
            var assingResponseSuccesful = await _service.AssingRole(requestDto.Email, requestDto.Role.ToUpper());
            if(!assingResponseSuccesful)
            {
                _response.IsSucess = false;
                _response.Message = "Error encountered";
                return BadRequest(_response);
            }
            _response.Result = assingResponseSuccesful;
            return Ok(_response);
        }

    }
}
