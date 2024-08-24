namespace Mango.Web.Models
{
	public class BaseResponseDto
	{
		public bool Success { get; set; }
		public string ErrorMessage { get; set; } = default!;
	}
	public class BaseResponseGeneric<T> : BaseResponseDto
	{
		public T Data { get; set; } = default!;
	}

	public class CollectionBaseRepsone<T> : BaseResponseDto
	{
		public ICollection<T>? Data { get; set; }
	}

	public class PaginationRepsonse<T> : BaseResponseDto
	{
		public ICollection<T>? Data { get; set; }
		public int Total { get; set; }
	}
}
