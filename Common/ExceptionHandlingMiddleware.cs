using System.Text.Json;

namespace DeskBooking.Api.Common;

/// <summary>
/// Глобальный middleware: превращает исключения в аккуратный JSON.
/// Это уменьшает мусорный boilerplate в контроллерах.
/// </summary>
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
            // В проде тут логирование.
            http.Response.StatusCode = 500;
            http.Response.ContentType = "application/json";
            await http.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = "Unexpected server error."
            }));
        }
    }
}

