
using static Mango.Web.Utility.SD;

namespace Mango.Web.Models
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; } = default!;
        public object Data { get; set; } = default!;
        public string AccesToken { get; set; } = default!;


        public ContentType ContentType { get; set; } = ContentType.Json;
    }
}
