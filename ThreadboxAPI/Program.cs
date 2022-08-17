using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ThreadboxAPI;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var assembly = Assembly.GetExecutingAssembly();

services.AddControllers();

// Database context
services.AddDbContext<ThreadboxContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Database");
    options.UseNpgsql(connectionString);
});

// Automapper
services.AddAutoMapper(assembly);

// FluentValidation
services.AddFluentValidation(config =>
{
    config.RegisterValidatorsFromAssembly(assembly);
});

// NSwag UI
services.AddSwaggerDocument(settings =>
{
    settings.Title = "ThreadboxAPI";
});

// CORS
services.AddCors(options =>
{
    options.AddPolicy("ThreadboxAPI CORS policy", builder =>
    {
        builder
            .WithOrigins("http://localhost:4200")
            .WithMethods("GET", "POST", "PUT", "DELETE")
            .Build();
    });
});

var app = builder.Build();

app.UseCors("ThreadboxAPI CORS policy");

if (app.Environment.IsDevelopment())
{
    // NSwag UI
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();