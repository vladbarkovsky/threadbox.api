﻿using System.Reflection;

namespace ThreadboxApi.Startup
{
    public class MediatRStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
        }
    }
}