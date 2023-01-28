using Services.Utils;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTelegramServices(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();