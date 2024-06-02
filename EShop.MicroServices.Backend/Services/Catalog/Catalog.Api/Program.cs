using Carter;
using Catalog.Api;
using Services.Shared.Middleware.Exceptions;

var builder = WebApplication.CreateBuilder(args)
	.AddAppSettings()
	.AddMiddleware()
	.AddValidators()
	.AddMediator()
	.AddCarterEndpoints();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

// Build app.
var app = builder.Build();

app.UseMiddleware<ExceptionsMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapCarter();

app.Run();