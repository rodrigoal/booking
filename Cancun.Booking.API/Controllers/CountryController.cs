
using Cancun.Booking.Application.Features.Countries.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cancun.Booking.API.Controllers
{

  [ApiController]
  [Route("api/country")]
  public class CountryController : ControllerBase
  {
    private readonly IMediator _mediator;

    public CountryController(IMediator mediator)
    {
      _mediator = mediator;

    }
    /// <summary>
    /// Returns the list of countries used to add an User (customer)
    /// </summary>
    /// <returns>List of countries</returns>
    [HttpGet("all", Name = "GetCountries")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CountryListVm>>> GetCountries()
    {

      var countries = await _mediator.Send(new GetCountryListQuery());
      return Ok(countries);
    }
  }
}
