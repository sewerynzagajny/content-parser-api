using ContentParserApi.ProgramSettings;


var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

app.MapApplication();

app.Run();