using Cancun.Booking.Application.Contracts.Persistence;
using Cancun.Booking.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.API.Tests.Mocks
{
  public class RepositoryMocks
  {
    public static Mock<IAsyncRepository<Room>> GetRoomRepository()
    {

      var rooms = new List<Room>()
      {
        new Room()
        {
           Id = 1,
        RoomNumber = "001"
        }

      };

      var mockRoomRepository = new Mock<IAsyncRepository<Room>>();
      mockRoomRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(rooms);

      return mockRoomRepository;
    }

    public static Mock<IAsyncRepository<Country>> GetCountryRepository()
    {

      var countries = new List<Country>()
      {
        new Country()
        {
           Id = 27,
           Name = "Brasil"
        }

      };

      var mockCountryRepository = new Mock<IAsyncRepository<Country>>();
      mockCountryRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(countries);

      return mockCountryRepository;
    }

    public static Mock<IBookingRepository> GetBookingRepository()
    {

      var bookings = new List<Reservation>()
      {
        new Reservation()
        {
          Id=1,
          BookingStartDate = DateTime.Now.Date.AddDays(1),
          BookingEndDate = DateTime.Now.Date.AddDays(3),
          RoomID = 1,
          CreatedDate = DateTime.Now,
          UserID = 1,
          BookingDetails = new List<BookingDetail>()
          {
            new BookingDetail()
            {
              Id = 1,
              BookingID = 1,
              BookingDate = DateTime.Now.Date.AddDays(1)
            },
            new BookingDetail()
            {
              Id = 2,
              BookingID = 1,
              BookingDate = DateTime.Now.Date.AddDays(2)
            },
            new BookingDetail()
            {
              Id = 3,
              BookingID = 1,
              BookingDate = DateTime.Now.Date.AddDays(3)
            }
          }
        }
      };

      var mockBookingRepository = new Mock<IBookingRepository>();
      mockBookingRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(bookings); 
      //mockBookingRepository.Setup(repo => repo.GetEmptyBookingsAsync(1)).ReturnsAsync()

      return mockBookingRepository;
    }
  }
}
