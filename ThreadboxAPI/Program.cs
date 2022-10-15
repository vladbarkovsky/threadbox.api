using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ThreadboxAPI;

namespace ThreadboxApi
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Startup.ConfigureServices(builder.Services, builder.Configuration);
            var app = builder.Build();
            Startup.Configure(app);
            app.Run();
        }
    }
}