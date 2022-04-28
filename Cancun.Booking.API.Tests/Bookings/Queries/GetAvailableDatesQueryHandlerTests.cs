using AutoMapper;
using Cancun.Booking.API.Controllers;
using Cancun.Booking.API.Tests.Mocks;
using Cancun.Booking.Application.Contracts.Persistence;
using Cancun.Booking.Application.Features.Bookings.Queries.GetAvailableDates;
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

namespace Cancun.Booking.API.Tests.Bookings.Queries
{
  public class GetAvailableDatesQueryHandlerTests
  {
    private readonly IMapper _mapper;
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly BookingController _bookingController;
    private readonly BookingContext _bookingContext;
    private DbContextOptions<BookingContext> _dbContextOptions;

    public GetAvailableDatesQueryHandlerTests()
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

      ConfigureDbContext();
    }

    private void ConfigureDbContext()
    {

      _bookingContext.Rooms.Add(new Room() { Id = 1, RoomNumber = "001" });

      _bookingContext.Countries.Add(new Country() { Id = 27, Name = "Brasil" });

      _bookingContext.Users.Add(new User() { Id = 1, Passport = "123", CountryID = 27 });


      _bookingContext.Bookings.Add(new Reservation()
      {
        Id = 1,
        RoomID = 1,
        CreatedDate = DateTime.Now,
        BookingStartDate = DateTime.Now.Date.AddDays(1),
        BookingEndDate = DateTime.Now.Date.AddDays(3),
        UserID = 1
      });
      _bookingContext.BookingDetails.Add(new BookingDetail()
      {
        BookingID = 1,
        BookingDate = DateTime.Now.Date.AddDays(1),
      });
      _bookingContext.BookingDetails.Add(new BookingDetail()
      {
        BookingID = 1,
        BookingDate = DateTime.Now.Date.AddDays(2),
      });
      _bookingContext.BookingDetails.Add(new BookingDetail()
      {
        BookingID = 1,
        BookingDate = DateTime.Now.Date.AddDays(3),
      });

      _bookingContext.SaveChanges();
    }

    [Fact]
    public async Task GetAvailableDatesTest()
    {
      var roomId = 1;
      var handler = new GetAvailableDatesQueryHandler(_bookingRepository);
      var result = await handler.Handle(new GetAvailableDatesQuery() {  RoomId = roomId }, System.Threading.CancellationToken.None);

      Assert.IsType<List<DateTime>>(result);
      Assert.NotEmpty(result);

    }

  }
}
