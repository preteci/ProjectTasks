using Azure.Core;
using BLL.Exceptions;

namespace WebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ProjectNotFoundException ex)
            {
                _logger.LogError($"There was an exception found: {ex.Message}");
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"{ex.Message}");
            }
            catch (TaskNotFoundException ex)
            {
                _logger.LogError($"There was an exception found: {ex.Message}");
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"{ex.Message}");
            }
            catch (TaskNotFoundOnProjectException ex)
            {
                _logger.LogError($"There was an exception found: {ex.Message}");
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"{ex.Message}");
            }
            catch (ProjectTasksNotCompletedException ex)
            {
                _logger.LogError($"There was an exception found: {ex.Message}");
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"{ex.Message}");
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError($"There was an exception found: {ex.Message}");
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"{ex.Message}");
            }
        }
    }
}
