using ApiAggregation.Application.Dtos;
using ApiAggregation.Application.Queries.AggregatedData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiAggregation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AggregatedDataController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AggregatedDataController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<AggregatedDataDto>> Get([FromQuery] string? sortBy, [FromQuery] string? filterBy)
        {
            try
            {
                var query = new GetAggregatedDataQuery(sortBy, filterBy);
                var result = await _mediator.Send(query);

                if (!result.WeatherData.Any() && !result.NewsData.Any() && !result.GithubData.Any())
                {
                    return NotFound("No data found matching the specified criteria");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorDto
                {
                    Message = "An error occurred while processing your request",
                    Source = "AggregatedDataController"
                });
            }
        }
    }
}
