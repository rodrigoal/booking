using System.ComponentModel.DataAnnotations.Schema;

namespace Cancun.Booking.API.Entities
{
  [Table("User")]
  public class User : Entity
  {

    public string Passport { get; set; }
    public int CountryID { get; set; }

  }
}
