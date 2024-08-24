namespace Mango.Services.ProductAPI.Model.Response
{
	public class ResponseDto
	{
		public object Result { get; set; } = default!;
		public bool IsSucess { get; set; }
		public string Message { get; set; } = "";
	}
}
