namespace Mango.Services.OrderApi.Models.Dtos
{
    public class ResponseDto
    {
        public object Result { get; set; } = default!;
        public bool IsSucess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
