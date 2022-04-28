using Cancun.Booking.Application.Contracts.Persistence;
using Cancun.Booking.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Persistence
{
  public static class PersistenceServiceRegistration
  {
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<BookingContext>(options => options.UseSqlServer(
          configuration.GetConnectionString("ConnectionStrings:BookingConnectionString")
        ));

      services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
      services.AddScoped<IRoomRepository, RoomRepository>();
      services.AddScoped<ICountryRepository, CountryRepository>();
      services.AddScoped<IBookingRepository, BookingRepository>();
      services.AddScoped<IUserRepository, UserRepository>();

      return services;
    }
  }
}
