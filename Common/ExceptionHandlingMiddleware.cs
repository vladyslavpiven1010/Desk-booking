using System.Text.Json;

namespace Desk_booking.Common;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext http)
    {
        try
        {
            await _next(http);
        }
        catch (BusinessException ex)
        {
            http.Response.StatusCode = ex.StatusCode;
            http.Response.ContentType = "application/json";
            await http.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = ex.Message
            }));
        }
        catch (Exception)
        {
            http.Response.StatusCode = 500;
            http.Response.ContentType = "application/json";
            await http.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = "Unexpected server error."
            }));
        }
    }
}

