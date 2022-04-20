using AutoMapper;
using Cancun.Booking.API.Models;
using Cancun.Booking.API.Repository;

namespace Cancun.Booking.API.Business
{
  public class BookingBusiness : IBookingBusiness
  {
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public BookingBusiness(IBookingRepository bookingRepository, IMapper mapper)
    {
      _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task AddBooking(BookingDto bookingDto)
    {


      //We could verify if the user already have some booking active. But I didn't here.

      try
      {

        await ValidateDates(bookingDto);

        var booking = _mapper.Map<Entities.Booking>(bookingDto);
        booking.UserID = _bookingRepository.GetUserID(bookingDto.UserPassport, bookingDto.CountryID);
        booking.CreatedDate = DateTime.Now;

        await _bookingRepository.AddBooking(booking);
      }
      catch (Exception)
      {
        throw;
      }

    }

 

    public async Task DeleteBooking(BookingDeleteDto bookingDto)
    {

      await ValidateBookingUser(new BookingDto() { ID = bookingDto.ID, UserPassport = bookingDto.UserPassport, CountryID = bookingDto.CountryID });

      var booking = await _bookingRepository.GetBookingAsync(bookingDto.ID);

      await _bookingRepository.DeleteBooking(booking);

    }


    public async Task<IEnumerable<BookingListDto>> GetBookingListAsync(string passport, int countryId)
    {

      var userId = _bookingRepository.GetUserID(passport, countryId);
      var bookingList = await _bookingRepository.GetBookingListAsync(userId);

      return _mapper.Map<IEnumerable<BookingListDto>>(bookingList);
    }

    public async Task<BookingDto> GetBookingAsync(int bookingId, string passport, int countryId)
    {
      try
      {
        await ValidateBookingUser(new BookingDto() { ID = bookingId, UserPassport = passport, CountryID = countryId });

        var booking = await _bookingRepository.GetBookingAsync(bookingId);

        return _mapper.Map<BookingDto>(booking);
      }
      catch (Exception)
      {

        throw;
      }

    }

    public IEnumerable<DateTime> GetEmptyDates()
    {

      var emptyDates = _bookingRepository.GetEmptyBookingsAsync();

      var lstDates = new List<DateTime>();
      foreach (var date in emptyDates)
        lstDates.Add(date.EmptyDate);

      return lstDates;

    }

    public async Task UpdateBooking(int bookingId, BookingDto bookingDto)
    {

      if (bookingDto == null || bookingDto.ID == null)
      {
        throw new ApplicationException("You must inform the ID of the booking to update it.");
      }

      await ValidateDates(bookingDto);

      //Verify if I can update that booking.
      await ValidateBookingUser(bookingDto);

      var bookingUpdate = _mapper.Map<Entities.Booking>(bookingDto);

      await _bookingRepository.UpdateBooking(bookingId, bookingUpdate);

    }

    private async Task ValidateBookingUser(BookingDto bookingDto)
    {
      var userId = _bookingRepository.GetUserID(bookingDto.UserPassport, bookingDto.CountryID);
      var booking = await _bookingRepository.GetBookingAsync(bookingDto.ID.Value);
      if (booking == null)
        throw new ApplicationException("The booking ID does not exist.");

      if (booking.UserID != userId)
        throw new ApplicationException("You can not update a booking that is not yours.");
    }

    private async Task ValidateDates(BookingDto bookingDto)
    {
      if (bookingDto.BookingStartDate > bookingDto.BookingEndDate)
      {
        throw new ApplicationException("The end date must be greater than start date.");
      }
      if ((bookingDto.BookingEndDate - bookingDto.BookingStartDate).TotalDays > 3)
      {
        throw new ApplicationException("Your reservation can not have more than 3 days. Sorry.");
      }

      if (await _bookingRepository.BookingExistsAsync(bookingDto.BookingStartDate, bookingDto.BookingEndDate))
      {
        throw new ApplicationException("Unfortunately some dates of reservation are not available for now.");
      }

      if (bookingDto.BookingStartDate < DateTime.Now.AddDays(1))
      {
        throw new ApplicationException("The bookings must start at least on the next day.");
      }
      if (bookingDto.BookingEndDate > DateTime.Now.AddDays(30))
      {
        throw new ApplicationException("The bookings must end at least next 30 days.");
      }
    }
  }
}
