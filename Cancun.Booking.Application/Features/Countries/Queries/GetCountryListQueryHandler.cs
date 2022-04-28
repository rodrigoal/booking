using AutoMapper;
using Cancun.Booking.Application.Contracts.Persistence;
using Cancun.Booking.Application.Features.Countries.Queries;
using Cancun.Booking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Application.Features.Countries.Queries
{
  public class GetCountryListQueryHandler : IRequestHandler<GetCountryListQuery, List<CountryListVm>>
  {
    private readonly IMapper _mapper;
    private readonly IAsyncRepository<Country> _countryRepository;

    public GetCountryListQueryHandler(IMapper mapper, IAsyncRepository<Country> countryRepository)
    {
      _mapper = mapper;
      _countryRepository = countryRepository;
    }

    public async Task<List<CountryListVm>> Handle(GetCountryListQuery request, CancellationToken cancellationToken)
    {
      var countries = (await _countryRepository.GetAllAsync()).ToList();
      return _mapper.Map<List<CountryListVm>>(countries);
    }
  }
}
