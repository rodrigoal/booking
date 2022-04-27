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
using Cancun.Booking.API.Business;

namespace Cancun.Booking.API.Controllers.Tests
{

  public class BookingControllerTests
  {

    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly BookingController _bookingController;
    private readonly IMapper _mapper;
    private readonly BookingContext _bookingContext;
    private DbContextOptions<BookingContext> _dbContextOptions;
    private readonly IBookingBusiness _bookingBusiness;


    public BookingControllerTests()
    {
      if (_mapper == null)
      {
        var mappingConfig = new MapperConfiguration(mc =>
        {
          mc.AddProfile(new BookingProfile());
          mc.AddProfile(new CountryProfile());
          mc.AddProfile(new RoomProfile());
        });
        IMapper mapper = mappingConfig.CreateMapper();
        _mapper = mapper;
      }

      var dbName = $"BookingDB_{DateTime.Now.ToFileTimeUtc()}";

      _dbContextOptions = new DbContextOptionsBuilder<BookingContext>()
          .UseInMemoryDatabase(dbName)
          .Options;

      _bookingContext = new BookingContext(_dbContextOptions);
      _bookingRepository = new BookingRepository(_bookingContext);
      _roomRepository = new RoomRepository(_bookingContext);
      _bookingBusiness = new BookingBusiness(_bookingRepository, _mapper, _roomRepository);
      _bookingController = new BookingController(_bookingBusiness);

      ConfigureDbContext();
    }

    private void ConfigureDbContext()
    {

      _bookingContext.Rooms.Add(new Entities.Room() { Id = 1, RoomNumber = "001" });

      _bookingContext.Countries.Add(new Entities.Country() { Id = 27, Name = "Brasil" });

      _bookingContext.Users.Add(new Entities.User() { Id = 1, Passport = "123", CountryID = 27 });


      _bookingContext.Bookings.Add(new Entities.Booking()
      {
        Id = 1,
        RoomID = 1,
        CreatedDate = DateTime.Now,
        BookingStartDate = DateTime.Now.Date.AddDays(1),
        BookingEndDate = DateTime.Now.Date.AddDays(3),
        UserID = 1
      });
      _bookingContext.BookingDetails.Add(new Entities.BookingDetail()
      {
        BookingID = 1,
        BookingDate = DateTime.Now.Date.AddDays(1),
      });
      _bookingContext.BookingDetails.Add(new Entities.BookingDetail()
      {
        BookingID = 1,
        BookingDate = DateTime.Now.Date.AddDays(2),
      });
      _bookingContext.BookingDetails.Add(new Entities.BookingDetail()
      {
        BookingID = 1,
        BookingDate = DateTime.Now.Date.AddDays(3),
      });

      _bookingContext.SaveChanges();
    }

    #region GetEmptyDates Tests

    [Fact()]
    public async void GetEmptyDates_ReturnsOK()
    {


      //Arrange
      int roomId = 1;

      //Act
      var result = await _bookingController.GetEmptyDates(roomId);

      //Assert

      Assert.IsType<OkObjectResult>(result.Result);
      var list = result.Result as OkObjectResult;
      var items = list.Value as List<DateTime>;
      Assert.NotEmpty(items);


    }

    [Fact()]
    public async void GetEmptyDates_NotFound()
    {

      FillAllBookingDates();
      //Arrange
      int roomId = 1;

      //Act
      var result = await _bookingController.GetEmptyDates(roomId);

      //Assert
      Assert.IsType<NotFoundResult>(result.Result);

    }

    private void FillAllBookingDates()
    {
      //Just to fill all dates. It is not a business rule.
      _bookingContext.Bookings.Add(new Entities.Booking()
      {
        Id = 2,
        RoomID = 1,
        CreatedDate = DateTime.Now,
        BookingStartDate = DateTime.Now.Date.AddDays(1),
        BookingEndDate = DateTime.Now.Date.AddDays(30),
        UserID = 1
      });
      DateTime dateToAdd = DateTime.Now.Date.AddDays(1);
      while (dateToAdd <= DateTime.Now.Date.AddDays(30))
      {
        _bookingContext.BookingDetails.Add(new Entities.BookingDetail()
        {
          BookingID = 2,
          BookingDate = dateToAdd,
        });

        dateToAdd = dateToAdd.AddDays(1);
      }

      _bookingContext.SaveChanges();
    }

    [Fact()]
    public async void GetEmptyDates_ReturnsBadRequest()
    {

      //Arrange
      int roomId = 2; //room does not exists

      //Act
      var result = await _bookingController.GetEmptyDates(roomId);

      //Assert
      Assert.IsType<BadRequestObjectResult>(result.Result);

    }

    #endregion

    #region GetBookingList Tests

    [Fact()]
    public async void GetBookingList_ReturnsOK()
    {

      //Arrange
      string passport = "123";
      int countryId = 27;

      //Act
      var result = await _bookingController.GetBookingList(passport, countryId);

      //Assert

      Assert.IsType<OkObjectResult>(result.Result);
      var list = result.Result as OkObjectResult;
      var items = list.Value as List<BookingListDto>;
      Assert.NotEmpty(items);

    }

    [Fact()]
    public async void GetBookingList_NotFound()
    {

      //Arrange
      string passport = "1234";
      int countryId = 27;

      //Act
      var result = await _bookingController.GetBookingList(passport, countryId);

      //Assert
      Assert.IsType<NotFoundResult>(result.Result);

    }

    #endregion

    #region GetBooking Tests

    [Fact()]
    public async void GetBooking_ReturnsOK_BadRequest()
    {

      //Arrange
      int bookingId = 1;
      string passport = "123";
      int countryId = 27;

      //Act
      var result = await _bookingController.GetBooking(bookingId, passport, countryId);

      //Assert
      if (result.Result is OkObjectResult)
      {
        Assert.IsType<OkObjectResult>(result.Result);
        var value = result.Result as OkObjectResult;
        var item = value.Value as BookingDto;
        Assert.Equal(bookingId, item.ID);
      }
      else
      {
        Assert.IsType<BadRequestObjectResult>(result.Result);
      }

    }

    #endregion

    #region AddBooking Tests

    [Fact()]
    public async void AddBooking_ReturnsCreatedAtRoute()
    {

      //Arrange
      BookingForCreationDto bookingDto = new BookingForCreationDto()
      {
        RoomID = 1,
        CountryID = 27,
        UserPassport = Guid.NewGuid().ToString(),
        BookingStartDate = DateTime.Now.Date.AddDays(4)
      };
      bookingDto.BookingEndDate = bookingDto.BookingStartDate.AddDays(2);

      //Act
      var result = await _bookingController.AddBooking(bookingDto);

      //Asssert
      Assert.IsType<CreatedAtRouteResult>(result);

      var item = result as CreatedAtRouteResult;
      Assert.IsType<BookingListDto>(item.Value);

      var bookingItem = item.Value as BookingListDto;
      Assert.Equal(bookingItem.RoomID, bookingDto.RoomID);
      Assert.Equal(bookingItem.BookingStartDate, bookingDto.BookingStartDate);
      Assert.Equal(bookingItem.BookingEndDate, bookingDto.BookingEndDate);


    }

    [Fact()]
    public async void AddBooking_BadRequest_NoDatesAvailable()
    {

      //Arrange
      BookingForCreationDto bookingDto = new BookingForCreationDto()
      {
        RoomID = 1,
        CountryID = 27,
        UserPassport = Guid.NewGuid().ToString(),
        BookingStartDate = DateTime.Now.Date.AddDays(1)
      };
      bookingDto.BookingEndDate = bookingDto.BookingStartDate.AddDays(3);

      //Act
      var result = await _bookingController.AddBooking(bookingDto);

      //Asssert

      //no dates available
      Assert.IsType<BadRequestObjectResult>(result);


    }


    [Fact()]
    public async void AddBooking_ReturnsBadRequest_MoreThan3Days()
    {

      //Arrange
      BookingForCreationDto bookingDto = new BookingForCreationDto()
      {
        RoomID = 1,
        CountryID = 27,
        UserPassport = "123",
        BookingStartDate = DateTime.Now.Date.AddDays(1),
        BookingEndDate = DateTime.Now.Date.AddDays(4)
      };

      //Act
      var result = await _bookingController.AddBooking(bookingDto);

      //Asssert
      Assert.IsType<BadRequestObjectResult>(result);

    }

    [Fact()]
    public async void AddBooking_ReturnsBadRequest_After30Days()
    {

      //Arrange
      BookingForCreationDto bookingDto = new BookingForCreationDto()
      {
        RoomID = 1,
        CountryID = 27,
        UserPassport = "123",
        BookingStartDate = DateTime.Now.Date.AddDays(29),
        BookingEndDate = DateTime.Now.Date.AddDays(31)
      };

      //Act
      var result = await _bookingController.AddBooking(bookingDto);

      //Asssert
      Assert.IsType<BadRequestObjectResult>(result);

    }

    [Fact()]
    public async void AddBooking_ReturnsBadRequest_RoomNotExists()
    {

      //Arrange
      BookingForCreationDto bookingDto = new BookingForCreationDto()
      {
        RoomID = 2,
        CountryID = 27,
        UserPassport = "123",
        BookingStartDate = DateTime.Now.Date.AddDays(29),
        BookingEndDate = DateTime.Now.Date.AddDays(31)
      };

      //Act
      var result = await _bookingController.AddBooking(bookingDto);

      //Asssert
      Assert.IsType<BadRequestObjectResult>(result);

    }

    #endregion


    #region UpdateBooking Tests

    [Fact()]
    public async void UpdateBooking_ReturnsNoContent()
    {

      //Arrange
      int bookingId = 1;
      BookingForUpdateDto bookingDto = new BookingForUpdateDto()
      {
        RoomID = 1,
        CountryID = 27,
        UserPassport = "123",
        BookingStartDate = DateTime.Now.Date.AddDays(10)
      };
      bookingDto.BookingEndDate = bookingDto.BookingStartDate.AddDays(2);


      //Act
      var result = await _bookingController.UpdateBooking(bookingId, bookingDto);

      //Asssert
      Assert.IsType<NoContentResult>(result);

    }

    [Fact()]
    public async void UpdateBooking_ReturnsBadRequest_BookingNotExists()
    {

      //Arrange
      int bookingId = 7;
      BookingForUpdateDto bookingDto = new BookingForUpdateDto()
      {
        RoomID = 1,
        CountryID = 27,
        UserPassport = Guid.NewGuid().ToString(),
        BookingStartDate = DateTime.Now.Date.AddDays(4)
      };
      bookingDto.BookingEndDate = bookingDto.BookingStartDate.AddDays(2);


      //Act
      var result = await _bookingController.UpdateBooking(bookingId, bookingDto);

      //Asssert
      Assert.IsType<BadRequestObjectResult>(result);

    }

    [Fact()]
    public async void UpdateBooking_ReturnsBadRequest_RoomNotExists()
    {

      //Arrange
      int bookingId = 17;
      BookingForUpdateDto bookingDto = new BookingForUpdateDto()
      {
        RoomID = 2,
        CountryID = 27,
        UserPassport = "123",
        BookingStartDate = DateTime.Now.Date.AddDays(1)
      };
      bookingDto.BookingEndDate = bookingDto.BookingStartDate.AddDays(2);


      //Act
      var result = await _bookingController.UpdateBooking(bookingId, bookingDto);

      //Asssert
      Assert.IsType<BadRequestObjectResult>(result);

    }

    #endregion

    #region DeleteBooking Tests

    [Fact()]
    public async void DeleteBooking_ReturnsBadRequest_BookingNotExists()
    {

      //Arrange
      BookingForDeleteDto bookingDto = new BookingForDeleteDto()
      {
        ID = 7,
        CountryID = 27,
        UserPassport = "123"
      };

      //Act
      var result = await _bookingController.DeleteBooking(bookingDto);

      //Asssert
      Assert.IsType<BadRequestObjectResult>(result);

    }


    [Fact()]
    public async void DeleteBooking_ReturnsNoContent()
    {

      //Arrange
      BookingForDeleteDto bookingDto = new BookingForDeleteDto()
      {
        ID = 1,
        CountryID = 27,
        UserPassport = "123"
      };

      //Act
      var result = await _bookingController.DeleteBooking(bookingDto);

      //Asssert
      Assert.IsType<NoContentResult>(result);

    }

    #endregion

  }
}