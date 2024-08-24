namespace Mango.Services.AuthAPI.Models.Dto
{
    public class ResponsDto
    {
        public object Result { get; set; } = default!;
        public bool IsSucess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
