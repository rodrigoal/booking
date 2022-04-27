using Cancun.Booking.API.Business;
using Cancun.Booking.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cancun.Booking.API.Controllers
{
  [ApiController]
  [Route("api/booking")]
  public class BookingController : ControllerBase
  {
    private readonly IBookingBusiness _bookingBusiness;

    public BookingController(IBookingBusiness bookingBusiness)
    {
      _bookingBusiness = bookingBusiness ?? throw new ArgumentNullException(nameof(bookingBusiness));

    }
    /// <summary>
    /// Returns the available dates for booking passing just the room id.
    /// </summary>
    /// <param name="roomId">Room ID</param>
    /// <returns>List of Dates</returns>
    [HttpGet]
    [Route("GetEmptyDates")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DateTime>>> GetEmptyDates(int roomId)
    {
      try
      {
        var emptyDates = await _bookingBusiness.GetEmptyDates(roomId);

        if (emptyDates.Count() == 0)
          return NotFound();

        return Ok(emptyDates);
      }
      catch (Exception ex)
      {

        return BadRequest(ex.Message);
      }


    }
    /// <summary>
    /// Get a list of booking of the customer passing the passport and country id.
    /// </summary>
    /// <param name="passport">User Passport</param>
    /// <param name="countryId">Country ID of the user</param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetBookingList/{passport}/{countryId}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BookingListDto>>> GetBookingList(string passport, int countryId)
    {
      var list = await _bookingBusiness.GetBookingListAsync(passport, countryId);
      if (list.Count() == 0)
        return NotFound();

      return Ok(list);
    }



    // Just did it above because of AddBooking to return CreatedAtRoute. 
    /// <summary>
    /// Get an already created booking passing the ID, user passport and countryId.
    /// </summary>
    /// <param name="bookingId">Booking ID</param>
    /// <param name="passport">User Passport</param>
    /// <param name="countryId">Country ID of the user</param>
    /// <returns></returns>
    [HttpGet("{bookingId}/{passport}/{countryId}", Name = "GetBooking")]
    //[Route("GetBooking/{bookingId}/{passport}/{countryId}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<BookingDto>> GetBooking(int bookingId, string passport, int countryId)
    {
      try
      {
        var booking = await _bookingBusiness.GetBookingAsync(bookingId, passport, countryId);

        return Ok(booking);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }

    }

    /// <summary>
    /// Method to create a reservation.
    /// </summary>
    /// <param name="bookingDto">Object with roomId, UserPassport and CountryID (to identify the customer), and Start and End dates.</param>
    /// <returns>A Route with object of booking if success, or BadRequest if occurred an error.</returns>
    [HttpPost]
    [Route("AddBooking")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddBooking(BookingForCreationDto bookingDto)
    {
      try
      {
        var booking = await _bookingBusiness.AddBooking(bookingDto);

        return CreatedAtRoute("GetBooking",
          new { bookingId = booking.ID, passport = bookingDto.UserPassport, countryId = bookingDto.CountryID },
          booking);
      }
      catch (Exception ex)
      {

        return BadRequest(ex.Message);
      }

    }
    /// <summary>
    /// Method to update a booking. Must pass the ID, and an object with roomId, UserPassport and CountryID, and the Start and End dates.
    /// </summary>
    /// <param name="bookingId">Booking ID</param>
    /// <param name="bookingDto">Object with roomId, UserPassport and CountryID, and the Start and End Dates</param>
    /// <returns>NoContent if success, or Badrequest if occurred an error</returns>
    [HttpPut]
    [Route("UpdateBooking")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateBooking(int bookingId, BookingForUpdateDto bookingDto)
    {
      try
      {
        await _bookingBusiness.UpdateBooking(bookingId, bookingDto);

        return NoContent();
      }
      catch (Exception ex)
      {

        return BadRequest(ex.Message);
      }

    }
    /// <summary>
    /// Method to delete a booking passing the booking ID, user passport and countryID.
    /// </summary>
    /// <param name="bookingDto">Object having the Id, UserPassport and CountryID</param>
    /// <returns>Returns NoContent if success, or BadRequest if occurred an error.</returns>
    [HttpDelete]
    [Route("DeleteBooking")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteBooking(BookingForDeleteDto bookingDto)
    {
      try
      {
        await _bookingBusiness.DeleteBooking(bookingDto);

        return NoContent();
      }
      catch (Exception ex)
      {

        return BadRequest(ex.Message);
      }

    }

  }
}
