using Services.Utils;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTelegramServices(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();