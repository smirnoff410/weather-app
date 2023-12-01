using Microsoft.AspNetCore.Mvc;
using System.Net;
using WeatherCommon.Services.Command;

namespace WeatherCommon.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ToActionResult(this HttpResponse result)
        {
            if (result.StatusCode == HttpStatusCode.OK)
            {
                if (result.Response == null)
                    return new StatusCodeResult((int)HttpStatusCode.OK);

                return new OkObjectResult(result.Response);
            }

            return new ObjectResult(result.StatusCode)
            {
                Value = result.Response
            };
        }
    }
}
