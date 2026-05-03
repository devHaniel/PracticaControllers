using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace GestionProducto.Application.Middlewares;

public class Middleware
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;

    public Middleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;        
        _logger = loggerFactory.CreateLogger(typeof(Middleware));
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // pasa al siguiente (controller)
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private static async Task HandleException(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var statusCode = ex switch
        {
            KeyNotFoundException => HttpStatusCode.NotFound,
            ArgumentException => HttpStatusCode.BadRequest,
            InvalidOperationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            message = ex.Message,
            status = context.Response.StatusCode
        };

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}
