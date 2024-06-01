using Carter;
using Catalog.Api;
using Services.Shared.Middleware.Exceptions;

var builder = WebApplication.CreateBuilder(args)
	.AddMiddleware()
	.AddMediator()
	.AddValidators()
	.AddCarterEndpoints();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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