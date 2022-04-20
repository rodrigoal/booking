using Cancun.Booking.API.Models;

namespace Cancun.Booking.API.Business
{
  public interface IBookingBusiness
  {

    IEnumerable<DateTime> GetEmptyDates(int roomId);

    Task<IEnumerable<BookingListDto>> GetBookingListAsync(string passport, int countryId);

    Task<BookingDto> GetBookingAsync(int bookingId, string passport, int countryId);

    Task<BookingListDto> AddBooking(BookingForCreationDto bookingDto);

    Task UpdateBooking(int bookingId, BookingForUpdateDto bookingDto);

    Task DeleteBooking(BookingForDeleteDto bookingDto);

  }
}
