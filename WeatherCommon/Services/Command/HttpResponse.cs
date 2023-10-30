using System.Net;

namespace WeatherCommon.Services.Command
{
    public class HttpResponse
    {
        public HttpStatusCode StatusCode { get; }
        public object? Response { get; }
        public HttpResponse(object response)
        {
            StatusCode = HttpStatusCode.OK;
            Response = response;
        }
        public HttpResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpResponse(HttpStatusCode statusCode, object? response) : this(statusCode)
        {
            Response = response;
        }
    }
}
