using Cancun.Booking.Application.Contracts.Persistence;
using Cancun.Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Persistence.Repositories
{
  public class BookingRepository : BaseRepository<Reservation>, IBookingRepository
  {
    public BookingRepository(BookingContext dbContext) : base(dbContext)
    {

    }

    public async Task<Reservation> AddBooking(Reservation booking)
    {
      _dbContext.Bookings.Add(booking);
      await _dbContext.SaveChangesAsync();


      //Generating details date table
      DateTime dateToAdd = booking.BookingStartDate;
      while (dateToAdd <= booking.BookingEndDate)
      {
        _dbContext.BookingDetails.Add(new BookingDetail()
        {
          BookingID = booking.Id,
          BookingDate = dateToAdd
        });
        dateToAdd = dateToAdd.Date.AddDays(1);
      }
      await _dbContext.SaveChangesAsync();

      return booking;
    }

    public async Task<bool> BookingExistsAsync(DateTime startDate, DateTime endDate, int? bookingId = null)
    {
      bool exists = false;
      if (bookingId != null)
        exists = await _dbContext.Bookings.AnyAsync(a => (a.Id != bookingId) && ((startDate >= a.BookingStartDate && startDate <= a.BookingEndDate) || (endDate >= a.BookingStartDate && endDate <= a.BookingEndDate)));
      else
        exists = await _dbContext.Bookings.AnyAsync(a => ((startDate >= a.BookingStartDate && startDate <= a.BookingEndDate) || (endDate >= a.BookingStartDate && endDate <= a.BookingEndDate)));

      return exists;
    }

    public async Task DeleteBooking(Reservation booking)
    {
      booking.BookingDetails.Clear();
      _dbContext.Bookings.Remove(booking);

      await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateBooking(Reservation booking)
    {
      booking.BookingDetails.Clear();

      _dbContext.Entry(booking).State = EntityState.Modified;
      await _dbContext.SaveChangesAsync();


      //Generating details date table
      DateTime dateToAdd = booking.BookingStartDate;
      while (dateToAdd <= booking.BookingEndDate)
      {
        _dbContext.BookingDetails.Add(new BookingDetail()
        {
          BookingID = booking.Id,
          BookingDate = dateToAdd
        });
        dateToAdd = dateToAdd.Date.AddDays(1);
      }
      await _dbContext.SaveChangesAsync();


    }

    public async Task<Reservation?> GetBookingAsync(int bookingId)
    {
      return await _dbContext.Bookings.Include(a=> a.BookingDetails).FirstOrDefaultAsync(a => a.Id == bookingId);
    }

    public async Task<Reservation?> GetBookingByStartDateAsync(DateTime startDate, int roomId)
    {
      return await _dbContext.Bookings.Include(a => a.BookingDetails).FirstOrDefaultAsync(a => a.BookingStartDate == startDate && a.RoomID == roomId);
    }

    public async Task<IEnumerable<Reservation>> GetBookingListAsync(int userId)
    {
      return await _dbContext.Bookings.Where(a => a.UserID == userId).ToListAsync();
    }

    public async Task<IEnumerable<DateTime>> GetEmptyBookingsAsync(int roomId)
    {
      DateTime startDate = DateTime.Now.Date.AddDays(1);
      DateTime dateToAdd = startDate;

      var bookingDetails = _dbContext.BookingDetails.Include(a => a.Booking)
                                    .Where(b => b.Booking.RoomID == roomId && b.BookingDate >= startDate);

      List<DateTime> lstDates = new List<DateTime>();
      for (int i = 0; i < 30; i++)
      {
        lstDates.Add(dateToAdd);  
        dateToAdd = dateToAdd.AddDays(1);
      }

      var freeDates = lstDates.Where(a => bookingDetails.All(b => b.BookingDate != a)).ToList();

      return freeDates;
    }

  }
}
