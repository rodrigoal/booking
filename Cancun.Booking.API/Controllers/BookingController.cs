
using Cancun.Booking.Application.Features.Bookings.Commands.AddBooking;
using Cancun.Booking.Application.Features.Bookings.Commands.DeleteBooking;
using Cancun.Booking.Application.Features.Bookings.Commands.UpdateBooking;
using Cancun.Booking.Application.Features.Bookings.Queries.GetAvailableDates;
using Cancun.Booking.Application.Features.Bookings.Queries.GetBookingList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cancun.Booking.API.Controllers
{
  [ApiController]
  [Route("api/booking")]
  public class BookingController : ControllerBase
  {

    private readonly IMediator _mediator;

    public BookingController(IMediator mediator)
    {
      _mediator = mediator;

    }
    /// <summary>
    /// Returns the available dates for booking passing just the room id.
    /// </summary>
    /// <param name="roomId">Room ID</param>
    /// <returns>List of Dates</returns>
    [HttpGet("getavailabledates/{roomId}", Name = "GetAvailableDates")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DateTime>>> GetAvailableDates(int roomId)
    {
      try
      {
        var emptyDates = await _mediator.Send(new GetAvailableDatesQuery() { RoomId = roomId });

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
    [HttpGet("getbookinglist/{passport}/{countryId}", Name = "GetBookingList")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<BookingListDto>>> GetBookingList(string passport, int countryId)
    {
      var list = await _mediator.Send(new GetBookingListQuery() { Passport = passport, CountryId = countryId });
      if (list.Count() == 0)
        return NotFound();

      return Ok(list);
    }



    //// Just did it above because of AddBooking to return CreatedAtRoute. 
    ///// <summary>
    ///// Get an already created booking passing the ID, user passport and countryId.
    ///// </summary>
    ///// <param name="bookingId">Booking ID</param>
    ///// <param name="passport">User Passport</param>
    ///// <param name="countryId">Country ID of the user</param>
    ///// <returns></returns>
    //[HttpGet("{bookingId}/{passport}/{countryId}", Name = "GetBooking")]
    ////[Route("GetBooking/{bookingId}/{passport}/{countryId}")]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //public async Task<ActionResult<BookingDto>> GetBooking(int bookingId, string passport, int countryId)
    //{
    //  try
    //  {
    //    var booking = await _bookingBusiness.GetBookingAsync(bookingId, passport, countryId);

    //    return Ok(booking);
    //  }
    //  catch (Exception ex)
    //  {
    //    return BadRequest(ex.Message);
    //  }

    //}

    /// <summary>
    /// Method to create a reservation.
    /// </summary>
    /// <param name="bookingDto">Object with roomId, UserPassport and CountryID (to identify the customer), and Start and End dates.</param>
    /// <returns>A Route with object of booking if success, or BadRequest if occurred an error.</returns>
    [HttpPost(Name = "AddBooking")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<AddBookingCommandResponse>> AddBooking([FromBody] AddBookingCommand addBookingCommand)
    {
      try
      {
        var booking = await _mediator.Send(addBookingCommand);

        return CreatedAtRoute("GetBooking",
          new { bookingId = booking.Booking.ID, passport = booking.Booking.UserPassport, countryId = booking.Booking.CountryID },
          booking.Booking);
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
    [HttpPut(Name = "UpdateBooking")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateBooking([FromBody] UpdateBookingCommand updateBookingCommand)
    {
      try
      {
        await _mediator.Send(updateBookingCommand);

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
    [HttpDelete(Name = "DeleteBooking")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteBooking([FromBody] DeleteBookingCommand deleteBookingCommand)
    {
      try
      {
        await _mediator.Send(deleteBookingCommand);

        return NoContent();
      }
      catch (Exception ex)
      {

        return BadRequest(ex.Message);
      }

    }

  }
}
