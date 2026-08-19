using Microsoft.AspNetCore.Http.HttpResults;
using Restaurants.Domain.Exceptions;

namespace Restaurant.API.Middlewares
{
    public class ErrorHandlingMiddle(ILogger<ErrorHandlingMiddle> loger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);

            }catch(NotFoundException notFound)
            {
                
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(notFound.Message);
                loger.LogWarning(notFound.Message);
            }
            catch (NameAlreadyExistsException ex)
            {

                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsJsonAsync(ex.Message);
                loger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                loger.LogError(ex, ex.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync("Something wrong");

            }
        }
    }
}
