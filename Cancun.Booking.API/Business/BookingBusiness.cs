using AutoMapper;
using Cancun.Booking.API.Models;
using Cancun.Booking.API.Repository;

namespace Cancun.Booking.API.Business
{
  public class BookingBusiness : IBookingBusiness
  {
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;
    private readonly IRoomRepository _roomRepository;

    public BookingBusiness(IBookingRepository bookingRepository, IMapper mapper, IRoomRepository roomRepository)
    {
      _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
    }

    public async Task<BookingListDto> AddBooking(BookingForCreationDto bookingForCreationDto)
    {

      try
      {
        var bookingDto = _mapper.Map<BookingDto>(bookingForCreationDto);

        //We could verify if the user already have some booking active. But I didn't here.
        await ValidateBooking(bookingDto);

        var booking = _mapper.Map<Entities.Booking>(bookingDto);
        booking.UserID = _bookingRepository.GetUserID(bookingDto.UserPassport, bookingDto.CountryID);
        booking.CreatedDate = DateTime.Now;

        await _bookingRepository.AddBooking(booking);

        var bookingCreated = await _bookingRepository.GetBookingByStartDateAsync(bookingDto.BookingStartDate, bookingForCreationDto.RoomID); //Get the added booking
        if (bookingCreated == null)
          throw new ApplicationException("Some error occurred to add this reservation.");

        var bookingDtoCreated = _mapper.Map<BookingListDto>(bookingCreated);

        return bookingDtoCreated;
      }
      catch (Exception)
      {
        throw;
      }

    }

    public async Task DeleteBooking(BookingForDeleteDto bookingDeleteDto)
    {

      var bookingDto = _mapper.Map<BookingDto>(bookingDeleteDto);
      await ValidateBookingUser(bookingDto);

      var booking = await _bookingRepository.GetBookingAsync(bookingDeleteDto.ID);

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

    public async Task<IEnumerable<DateTime>> GetEmptyDates(int roomId)
    {

      if ((await _roomRepository.ExistsAsync(roomId)) == false)
        throw new ApplicationException("This room does not exists. Choose another one.");

      var emptyDates = _bookingRepository.GetEmptyBookingsAsync(roomId);
      var lstDates = new List<DateTime>();
      foreach (var date in emptyDates)
        lstDates.Add(date.EmptyDate);

      return lstDates;

    }

    public async Task UpdateBooking(int bookingId, BookingForUpdateDto bookingForUpdateDto)
    {


      var bookingDto = _mapper.Map<BookingDto>(bookingForUpdateDto);
      bookingDto.ID = bookingId;

      await ValidateBooking(bookingDto);

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
        throw new ApplicationException("You can not update/delete a booking that not yours.");
    }

    private async Task ValidateBooking(BookingDto bookingDto)
    {

      if ((await _roomRepository.ExistsAsync(bookingDto.RoomID)) == false)
        throw new ApplicationException("This room does not exists. Choose another one.");

      if (bookingDto.BookingStartDate > bookingDto.BookingEndDate)
      {
        throw new ApplicationException("The end date must be greater than start date.");
      }
      if ((bookingDto.BookingEndDate - bookingDto.BookingStartDate).TotalDays > 2)
      {
        throw new ApplicationException("Your reservation can not have more than 3 days. Sorry.");
      }

      if (await _bookingRepository.BookingExistsAsync(bookingDto.BookingStartDate, bookingDto.BookingEndDate, bookingDto.ID))
      {
        throw new ApplicationException("Unfortunately some dates of reservation are not available for now.");
      }

      if (bookingDto.BookingStartDate < DateTime.Now.Date.AddDays(1))
      {
        throw new ApplicationException("The bookings must start at least on the next day.");
      }
      if (bookingDto.BookingEndDate > DateTime.Now.Date.AddDays(30))
      {
        throw new ApplicationException("The bookings must end at least next 30 days.");
      }
    }
  }
}
