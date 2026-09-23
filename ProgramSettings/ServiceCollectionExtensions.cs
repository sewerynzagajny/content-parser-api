using ContentParserApi.GlobalExceptions;
using ContentParserApi.Services;
using ContentParserApi.Strategies;
using ContentParserApi.Validations;


namespace ContentParserApi.ProgramSettings
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddAuthorization();
            services.AddApiControlers();
            services.AddProblemDetails();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddOpenApi();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddScoped<IParserStrategy, CsvParseStrategy>();
            services.AddScoped<IParserStrategy, InternalJsonParseStrategy>();
            services.AddScoped<Validation>();
            services.AddScoped<ParseService>();
            services.AddScoped<DecodeBase64Service>();

            return services;

        }
    }
}
