using Cancun.Booking.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cancun.Booking.API.Repository;
using AutoMapper;
using Cancun.Booking.API.Profiles;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Cancun.Booking.API.Models;
using Cancun.Booking.API.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Cancun.Booking.API.Controllers.Tests
{

  public class CountryControllerTests
  {

    private readonly ICountryRepository _countryRepository;
    private readonly CountryController _countryController;
    private readonly IMapper _mapper;
    private readonly BookingContext _bookingContext;
    private DbContextOptions<BookingContext> _dbContextOptions;


    public CountryControllerTests()
    {
      if (_mapper == null)
      {
        var mappingConfig = new MapperConfiguration(mc =>
        {
          //mc.AddProfile(new BookingProfile());
          mc.AddProfile(new CountryProfile());
          //mc.AddProfile(new RoomProfile());
        });
        IMapper mapper = mappingConfig.CreateMapper();
        _mapper = mapper;
      }


      var dbName = $"BookingDB_{DateTime.Now.ToFileTimeUtc()}";

      _dbContextOptions = new DbContextOptionsBuilder<BookingContext>()
          .UseInMemoryDatabase(dbName)
          .Options;


      _bookingContext = new BookingContext(_dbContextOptions);
      _countryRepository = new CountryRepository(_bookingContext);
      _countryController = new CountryController(_countryRepository, _mapper);

      ConfigureDbContext();
    }

    private void ConfigureDbContext()
    {

      _bookingContext.Countries.Add(new Entities.Country() { Id = 27, Name = "Brasil" });
      _bookingContext.SaveChanges();

    }

    [Fact()]
    public async void GetCountries_ReturnsOK()
    {

      //Arrange
      //Act
      var result = await _countryController.GetCountries();

      //Assert
      Assert.IsType<OkObjectResult>(result.Result);

    }
    [Fact()]
    public async void GetCountries_ReturnsItems()
    {

      //Arrange
      //Act
      var result = await _countryController.GetCountries();

      var list = result.Result as OkObjectResult;
      //Assert
      Assert.IsType<List<CountryDto>>(list.Value);

      var items = list.Value as List<CountryDto>;
      //Assert
      Assert.NotEmpty(items);


    }
  }
}