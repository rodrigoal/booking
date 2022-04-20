using AutoMapper;
using Cancun.Booking.API.Models;
using Cancun.Booking.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Cancun.Booking.API.Controllers
{

  [ApiController]
  [Route("api/country")]
  public class CountryController : ControllerBase
  {
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public CountryController(ICountryRepository countryRepository, IMapper mapper)
    {
      _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      
    }

    [HttpGet]
    [Route("GetCountries")]
    public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountries()
    {

      var countries = await _countryRepository.GetCountriesAsync();

      return Ok(_mapper.Map<IEnumerable<CountryDto>>(countries));
    }
  }
}
