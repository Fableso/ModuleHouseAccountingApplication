using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Web.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
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
        catch (EntityAlreadyExistsException ex)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Entity Already Exists",
                Status = StatusCodes.Status409Conflict,
                Type = "https://httpstatuses.com/409",
                Detail = ex.Message
            };
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (Exception e)
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