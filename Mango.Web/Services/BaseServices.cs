using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;
using static Mango.Web.Utility.SD;


namespace Mango.Web.Services
{
    public class BaseServices : IBaseServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;

        public BaseServices(IHttpClientFactory httpClientFactory,
            ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }

        public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("MangoApi");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                if (requestDto.ContentType == ContentType.MultipartFormData)
                {
                    message.Headers.Add("Accept", "*/*");
                }
                else
                {
                    message.Headers.Add("Accept", "application/json");
                }



                //Token add
                if (withBearer)
                {
                    var token = _tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }

                message.RequestUri = new Uri(requestDto.Url);

                if (requestDto.ContentType == ContentType.MultipartFormData)
                {
                    var content = new MultipartFormDataContent();
                    foreach (var prop in requestDto.Data.GetType().GetProperties())
                    {
                        var value = prop.GetValue(requestDto.Data);
                        if(value is FormFile)
                        {
                            var file = (FormFile)value;
                            if(file != null)
                            {
                                content.Add(new StreamContent(file.OpenReadStream()),prop.Name,file.FileName);
                            }
                        }
                        else
                        {
                            content.Add(new StringContent(value == null ? "": value.ToString()), prop.Name);

                        }
                    }
                }
                else
                {

                    if (requestDto.Data is not null)
                    {
                        message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");

                    }
                }

                HttpResponseMessage? apiresponse = null;

                switch (requestDto.ApiType)
                {
                    case Utility.SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case Utility.SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case Utility.SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }


                apiresponse = await client.SendAsync(message);

                switch (apiresponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return new() { IsSucess = false, Message = "Not Found" };
                    case System.Net.HttpStatusCode.Forbidden:
                        return new() { IsSucess = false, Message = "Forbidden" };
                    case System.Net.HttpStatusCode.Unauthorized:
                        return new() { IsSucess = false, Message = "Unauthorized" };
                    case System.Net.HttpStatusCode.InternalServerError:
                        return new() { IsSucess = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await apiresponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }
            catch (Exception ex)
            {

                var dto = new ResponseDto()
                {
                    Message = ex.Message.ToString(),
                    IsSucess = false,
                };
                return dto;

            }


        }
    }
}
