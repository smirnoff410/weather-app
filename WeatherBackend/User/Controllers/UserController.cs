using Microsoft.AspNetCore.Mvc;

namespace WeatherBackend.User.Controllers
{
    using System;
    using WeatherBackend.Services.WeatherService;
    using WeatherBackend.History.Models;
    using WeatherCommon.Services.Mediator;
    using WeatherCommon.Models.Request;
    using WeatherBackend.User.Models;
    using WeatherCommon.Extensions;

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _mediator.Dispatch<GetEntitiesRequest, ICollection<UserResponseItem>>(new GetEntitiesRequest());
            return result.ToActionResult();
        }

    }
}
