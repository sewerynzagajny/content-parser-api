using ContentParserApi.ProgramSettings;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();

var app = builder.Build();

app.MapApplication();

app.Run();  