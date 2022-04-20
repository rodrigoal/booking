namespace Cancun.Booking.API.Entities
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
