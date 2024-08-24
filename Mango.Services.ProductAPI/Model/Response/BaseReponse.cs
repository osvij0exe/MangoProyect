namespace Mango.Services.ProductAPI.Model.Response
{
    public class BaseReponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = default!;
    }
    public class BaseResponseGeneric<T>:BaseReponse
    {
        public T Data { get; set; } = default!;
    }

    public class CollectionBaseRepsone<T> : BaseReponse
    {
        public ICollection<T>? Data { get; set; }
    }

    public class PaginationRepsonse<T> : BaseReponse
    {
        public ICollection<T>? Data { get; set; }
        public int Total { get; set; }
    }

}
