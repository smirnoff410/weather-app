using Microsoft.AspNetCore.Mvc;

namespace WeatherBackend.City.Controllers
{
    using WeatherBackend.City.Models;
    using WeatherCommon.Extensions;
    using WeatherCommon.Models.Request;
    using WeatherCommon.Services.Mediator;

    [ApiController]
    [Route("[controller]")]
    public class CityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CityController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetCities()
        {
            var result = await _mediator.Dispatch<GetEntitiesRequest, ICollection<CityResponseItem>>(new GetEntitiesRequest());
            return result.ToActionResult();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateCityDTO dto)
        {
            var result = await _mediator.Dispatch<CreateEntityRequest<CreateCityDTO>, CityResponseItem>(new CreateEntityRequest<CreateCityDTO>(dto));
            return result.ToActionResult();
        }

        [HttpPut("[action]/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateCityDTO dto)
        {
            var result = await _mediator.Dispatch<UpdateEntityRequest<UpdateCityDTO>, CityResponseItem>(new UpdateEntityRequest<UpdateCityDTO>(id, dto));
            return result.ToActionResult();
        }

        [HttpDelete("[action]/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Dispatch<DeleteEntityRequest, CityResponseItem>(new DeleteEntityRequest(id));
            return result.ToActionResult();
        }
    }
}
