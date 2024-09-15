using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Web.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (EntityNotFoundException ex)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Searched Entity Not Found",
                Status = StatusCodes.Status404NotFound,
                Type = "https://httpstatuses.com/404",
                Detail = ex.Message
            };
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (Exception)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://httpstatuses.com/500"
            };
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}