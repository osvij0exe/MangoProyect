namespace Mango.Services.AuthAPI.Models.Dto
{
    public class LoginRequestDto
    {
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;

    }
}
