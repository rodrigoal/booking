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

        var bookingDtoCreated = _mapper.Map<BookingListDto>(booking);

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
      await ValidateBookingUpdateDelete(bookingDto);

      var booking = await _bookingRepository.GetBookingAsync(bookingDeleteDto.ID);
      if (booking != null)
      {
        _bookingRepository.DeleteBooking(booking);
        await _bookingRepository.SaveChangesAsync();
      }


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
        await ValidateBookingUpdateDelete(new BookingDto() { ID = bookingId, UserPassport = passport, CountryID = countryId });

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

      return emptyDates;

    }

    public async Task UpdateBooking(int bookingId, BookingForUpdateDto bookingForUpdateDto)
    {

      var bookingDto = _mapper.Map<BookingDto>(bookingForUpdateDto);
      bookingDto.ID = bookingId;

      await ValidateBooking(bookingDto);
      //Verify if I can update that booking.
      await ValidateBookingUpdateDelete(bookingDto);

      var bookingEntity = await _bookingRepository.GetBookingAsync(bookingId);

      _mapper.Map(bookingDto, bookingEntity);

      await _bookingRepository.SaveChangesAsync();


    }

    private async Task ValidateBookingUpdateDelete(BookingDto bookingDto)
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
