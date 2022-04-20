using System.ComponentModel.DataAnnotations.Schema;

namespace Cancun.Booking.API.Entities
{
  [Table("Country")]
  public class Country : Entity  
  {
    public string Name { get; set; }

  }
}
