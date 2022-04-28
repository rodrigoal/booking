using Cancun.Booking.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Cancun.Booking.Application.Contracts.Persistence;
using Cancun.Booking.Persistence;
using Cancun.Booking.Application.Profiles;
using Cancun.Booking.Persistence.Repositories;

namespace Cancun.Booking.API.Controllers.Tests
{

  public class RoomControllerTests
  {
    //private IConfigurationRoot _configuration;
    private readonly IRoomRepository _roomRepository;
    private readonly RoomController _roomController;
    private readonly IMapper _mapper;
    private readonly BookingContext _bookingContext;
    private DbContextOptions<BookingContext> _dbContextOptions;


    public RoomControllerTests()
    {
      if (_mapper == null)
      {
        var mappingConfig = new MapperConfiguration(mc =>
        {
          //mc.AddProfile(new BookingProfile());
          //mc.AddProfile(new CountryProfile());
          mc.AddProfile(new RoomProfile());
        });
        IMapper mapper = mappingConfig.CreateMapper();
        _mapper = mapper;
      }

      //var builder = new ConfigurationBuilder()
      //        .SetBasePath(Directory.GetCurrentDirectory())
      //        .AddJsonFile("appsettings.json");

      //_configuration = builder.Build();

      var dbName = $"BookingDB_{DateTime.Now.ToFileTimeUtc()}";

      _dbContextOptions = new DbContextOptionsBuilder<BookingContext>()
          .UseInMemoryDatabase(dbName)
          .Options;

      _bookingContext = new BookingContext(_dbContextOptions);
      _roomRepository = new RoomRepository(_bookingContext);
      //_roomController = new RoomController(_roomRepository, _mapper);


      ConfigureDbContext();
    }

    private void ConfigureDbContext()
    {

      //_bookingContext.Rooms.Add(new Entities.Room() { Id = 1, RoomNumber = "001" });
      //_bookingContext.SaveChanges();

    }

    [Fact()]
    public async void GetRooms_ReturnsOK()
    {

      //Arrange
      //Act
      var result = await _roomController.GetRooms();

      //Assert
      Assert.IsType<OkObjectResult>(result.Result);

    }
    [Fact()]
    public async void GetRooms_ReturnsItems()
    {

      ////Arrange
      ////Act
      //var result = await _roomController.GetRooms();

      //var list = result.Result as OkObjectResult;
      ////Assert
      //Assert.IsType<List<RoomDto>>(list.Value);

      //var items = list.Value as List<RoomDto>;
      ////Assert
      //Assert.Single(items);


    }
  }
}