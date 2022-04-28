namespace Cancun.Booking.Domain.Common
{
  public abstract class Entity
  {
    public virtual int? Id
    {
      get;
      set;
    }

    public Entity()
    {
    }

    public Entity(int id)
    {
      Id = id;
    }
  }
}
