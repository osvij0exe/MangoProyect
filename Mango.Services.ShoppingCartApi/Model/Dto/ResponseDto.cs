namespace Mango.Services.ShoppingCartApi.Model.Dto
{
    public class ResponseDto
    {
        public object Result { get; set; } = default!;
        public bool IsSucess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
