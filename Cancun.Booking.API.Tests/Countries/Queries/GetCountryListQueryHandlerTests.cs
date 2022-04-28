using AutoMapper;
using Cancun.Booking.API.Tests.Mocks;
using Cancun.Booking.Application.Contracts.Persistence;
using Cancun.Booking.Application.Features.Countries.Queries;
using Cancun.Booking.Application.Profiles;
using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Persistence;
using Cancun.Booking.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cancun.Booking.API.Tests.Countries.Queries
{
  public class GetCountryListQueryHandlerTests
  {
    private readonly IMapper _mapper;
    private readonly ICountryRepository _countryRepository;
    private readonly BookingContext _bookingContext;
    private DbContextOptions<BookingContext> _dbContextOptions;

    public GetCountryListQueryHandlerTests()
    {

      var configurationProvider = new MapperConfiguration(cfg =>
      {
        cfg.AddProfile<CountryProfile>();
      });
      _mapper = configurationProvider.CreateMapper();

      var dbName = $"BookingDB_{DateTime.Now.ToFileTimeUtc()}";

      _dbContextOptions = new DbContextOptionsBuilder<BookingContext>()
      .UseInMemoryDatabase(dbName)
      .Options;

      _bookingContext = new BookingContext(_dbContextOptions);
      _countryRepository = new CountryRepository(_bookingContext);

      ConfigureDbContext();

    }

    private void ConfigureDbContext()
    {

      _bookingContext.Countries.Add(new Country() { Id = 27, Name = "Brasil" });
      _bookingContext.SaveChanges();

    }

    [Fact]
    public async Task GetCountryListTest()
    {

      var handler = new GetCountryListQueryHandler(_mapper, _countryRepository);
      var result = await handler.Handle(new GetCountryListQuery(), System.Threading.CancellationToken.None);

      Assert.IsType<List<CountryListVm>>(result);
      Assert.Single(result);

    }

  }
}
