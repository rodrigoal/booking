using Cancun.Booking.Application.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace Cancun.Booking.API.Middleware
{
  public class ExceptionHandlerMiddleware
  {
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        await ConvertException(context, ex);
      }
    }

    private Task ConvertException(HttpContext context, Exception ex)
    {
      HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

      context.Response.ContentType = "application/json";

      var result = string.Empty;
      switch (ex)
      {
        case ValidationException validationException:
          httpStatusCode = HttpStatusCode.BadRequest;
          result = JsonConvert.SerializeObject(validationException.ValidationErrors);
          break;
        case BadRequestException badRequestException:
          httpStatusCode = HttpStatusCode.BadRequest;
          result = badRequestException.Message;
          break;
        case NotFoundException notFoundException:
          httpStatusCode = HttpStatusCode.NotFound;
          break;
        default:
          httpStatusCode = HttpStatusCode.BadRequest;
          break;
      }

      context.Response.StatusCode = (int)httpStatusCode;
      if (String.IsNullOrEmpty(result))
      {
        result = JsonConvert.SerializeObject(new { error = ex.Message });
      }

      return context.Response.WriteAsync(result);
    }

  }
}
