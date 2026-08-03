using ContentParserApi.DTOs;
using CsvHelper;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace ContentParserApi.GlobalExceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            this.logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var exceptionMessage = exception.Message;
            logger.LogError(
                "Error Message: {exceptionMessage}, Time of occurrence {time}",
                exceptionMessage,
                DateTime.UtcNow);

            int statusCode = StatusCodes.Status500InternalServerError;
            string message = "Internal Server Error";

            if (exception is ArgumentException || exception is FormatException || exception is CsvHelperException || exception is JsonException)
            {
                statusCode = StatusCodes.Status400BadRequest;
                message = exceptionMessage;
            }

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(new ApiErrorDto
            {
                Status = "Error",
                ErrorMessage = message
            }, cancellationToken);

            return true;
        }
    }
}
