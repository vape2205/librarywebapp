using librarwebapp.Exceptions;
using Microsoft.AspNetCore.Authentication;

namespace librarwebapp.Infraestructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
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
                if(ex is TokenExpiredApiException)
                {
                    await context.ChallengeAsync();
                    return;
                }
                // Log the exception or handle it as needed
                // Example: Return a custom error response
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("An error occurred: " + ex.Message);
            }
        }
    }
}
