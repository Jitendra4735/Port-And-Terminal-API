using System.Net;
using WebApi.Utilities;

namespace WebApi.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next; // Delegate to the next middleware in the pipeline.
        private readonly ILogger<ExceptionHandlingMiddleware> _logger; // Logger instance for logging exceptions.

        /// <summary>
        /// Constructor with dependency injection for the next middleware and logger.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Main method to handle HTTP requests and catch exceptions.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                _logger.LogInformation("Processing request: {Method} {Path}", context.Request.Method, context.Request.Path);

                // Calls the next middleware in the pipeline.
                await _next(context);

                _logger.LogInformation("Successfully processed request: {Method} {Path}", context.Request.Method, context.Request.Path);
            }
            catch (HttpClientException ex)
            {
                _logger.LogWarning("HttpClientException caught: {Message}", ex.Message);
                // Handles specific HttpClientException exceptions.
                await HandleHttpClientExceptions(context, ex);
            }
            catch (Exception ex)
            {
                // Logs the details of any unhandled exceptions.
                _logger.LogError(ex, "An unhandled exception occurred while processing request: {Method} {Path}", context.Request.Method, context.Request.Path);

                // Sets the response content type and status code for unexpected errors.
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Creates a generic error response.
                var errorResponse = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An unexpected error occurred. Please try again later."
                };

                // Writes the error response to the response body.
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }

        /// <summary>
        /// Handles HttpClientException specific errors.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private async Task HandleHttpClientExceptions(HttpContext context, HttpClientException exception)
        {
            _logger.LogInformation($"Handling HttpClientException **STARTS** with HttpContext = {context.ToString()}, HttpClientException = {exception.ToString()}");

            // Sets the response status code from the exception.
            context.Response.StatusCode = (int)exception.HttpResponseMessage.StatusCode;
            // Sets the response content type from the exception's HTTP response.
            context.Response.ContentType = exception.HttpResponseMessage.Content.Headers?.ContentType.ToString();
            // Reads the error message from the exception's HTTP response content.
            var error = await exception.HttpResponseMessage.Content.ReadAsStringAsync();
            // Writes the error message to the response body.
            await context.Response.WriteAsync(error);

            _logger.LogInformation($"HttpClientException handled with response Status code: {context.Response.StatusCode}");
        }
    }
}
