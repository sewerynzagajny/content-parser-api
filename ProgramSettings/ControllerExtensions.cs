using ContentParserApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ContentParserApi.ProgramSettings
{
    public static class ControllerExtensions
    {

        public static IServiceCollection AddApiControlers(this IServiceCollection services) 
        {
            services.AddControllers().ConfigureApiBehaviorOptions(opitions =>
            {
                //opitions.SuppressModelStateInvalidFilter = true;
                opitions.InvalidModelStateResponseFactory = contex =>
                {
                    var error = new ApiErrorDto
                    {
                        Status = "Error",
                        ErrorMessage = "Invalid request payload. Incorrect body format or type"
                    };
                    return new BadRequestObjectResult(error);
                };
            });
            return services;
        }
    }
}
