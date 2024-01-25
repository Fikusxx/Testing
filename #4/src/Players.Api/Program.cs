using Players.Api.Middlewares;
using Players.Infrastructure;
using Players.Application;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterApplicationServices();
builder.Services.RegisterInfrastructureServices();

builder.Services.AddAuthentication("TestScheme");
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();


app.MapControllers();

app.Run();
