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
    
    [HttpGet]
    [Route("GetEmptyDates")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<DateTime>> GetEmptyDates()
    {

      var emptyDates = _bookingBusiness.GetEmptyDates();

      if (emptyDates == null)
      {
        return NotFound();
      }

      return Ok(emptyDates);

    }

    [HttpGet]
    [Route("GetBookingList/{passport}/{countryId}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BookingListDto>>> GetBookingList(string passport, int countryId)
    {
      var list = await _bookingBusiness.GetBookingListAsync(passport, countryId);

      return Ok(list);
    }

    [HttpGet]
    [Route("GetBooking/{bookingId}/{passport}/{countryId}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetBooking(int bookingId, string passport, int countryId)
    {
      var booking = await _bookingBusiness.GetBookingAsync(bookingId, passport, countryId);
      
      return Ok(booking);
    }


    [HttpPost]
    [Route("AddBooking")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddBooking(BookingDto bookingDto)
    {
      try
      {
         await _bookingBusiness.AddBooking(bookingDto);

        return Ok();

        //TODO: Change code above to work with new ID of created booking.
        //return CreatedAtRoute("GetBooking", new { ID = bookingDto.ID });
      }
      catch (Exception ex)
      {

        return BadRequest(ex.Message);
      }

    }

    [HttpPut]
    [Route("UpdateBooking")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateBooking(int bookingId, BookingDto bookingDto)
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

    [HttpDelete]
    [Route("DeleteBooking")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteBooking(BookingDeleteDto bookingDto)
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
