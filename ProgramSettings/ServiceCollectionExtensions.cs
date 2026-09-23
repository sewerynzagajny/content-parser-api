using ContentParserApi.GlobalExceptions;
using ContentParserApi.Services;


namespace ContentParserApi.ProgramSettings
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
          

           services.AddApiControlers();
           services.AddProblemDetails();
           services.AddExceptionHandler<GlobalExceptionHandler>();
           services.AddOpenApi();
           services.AddEndpointsApiExplorer();
           services.AddSwaggerGen();
           services.AddScoped<ParseService>();
           services.AddScoped<DecodeBase64Service>();

           return services;

        }
    }
}
