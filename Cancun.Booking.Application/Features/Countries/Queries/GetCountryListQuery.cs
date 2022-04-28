using Cancun.Booking.Application.Features.Countries.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Application.Features.Countries.Queries
{
  public class GetCountryListQuery : IRequest<List<CountryListVm>>
  {
 
  } 
}
