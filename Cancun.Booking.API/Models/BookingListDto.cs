﻿namespace Cancun.Booking.API.Models
{
  public class BookingListDto
  {
    public int ID { get; set; }
    public int RoomID { get; set; }
    public DateTime BookingStartDate { get; set; }
    public DateTime BookingEndDate { get; set; }
    public DateTime CreatedDate { get; set; }
  }
}
