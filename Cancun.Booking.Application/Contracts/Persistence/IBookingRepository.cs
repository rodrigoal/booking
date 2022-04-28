using Cancun.Booking.Domain.Entities;

namespace Cancun.Booking.Application.Contracts.Persistence
{
  public interface IBookingRepository : IAsyncRepository<Domain.Entities.Reservation>
  {
    Task<IEnumerable<Domain.Entities.Reservation>> GetBookingListAsync(int userId);
    Task<Domain.Entities.Reservation?> GetBookingAsync(int bookingId);
    Task<IEnumerable<DateTime>> GetEmptyBookingsAsync(int roomId);
    Task<bool> BookingExistsAsync(DateTime startDate, DateTime endDate, int? bookingId = null);

    Task<Domain.Entities.Reservation?> GetBookingByStartDateAsync(DateTime startDate, int roomId);

    Task AddBooking(Domain.Entities.Reservation booking);
    void DeleteBooking(Domain.Entities.Reservation booking);


  }
}
