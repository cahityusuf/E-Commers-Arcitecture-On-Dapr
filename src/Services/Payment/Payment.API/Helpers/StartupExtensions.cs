using Dapr.Client;
using ECommers.Dapr;
using ECommers.Dapr.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Order.API.Helpers
{
    public static class StartupExtensions
    {
        private const string AppName = "Payment API";
        public static WebApplicationBuilder AddPaymentApi(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers().AddDapr();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.ConfigureDapr();
            builder.AddCustomApplicationServices();
            builder.AddCustomSerilog();
            return builder;
        }

        public static void AddCustomSerilog(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.Console()
                .WriteTo.File("./Logs/log.json", rollingInterval: RollingInterval.Day)
                .WriteTo.RabbitMQ((clientConfiguration, sinkConfiguration) =>
                {
                    clientConfiguration.Username = builder.Configuration["Logging:Serilog:RabbitMq:RABBITMQ_USER"];
                    clientConfiguration.Password = builder.Configuration["Logging:Serilog:RabbitMq:RABBITMQ_PASSWORD"];
                    clientConfiguration.Exchange = builder.Configuration["Logging:Serilog:RabbitMq:RABBITMQ_EXCHANGE"];
                    clientConfiguration.ExchangeType = builder.Configuration["Logging:Serilog:RabbitMq:RABBITMQ_EXCHANGE_TYPE"];
                    clientConfiguration.DeliveryMode = RabbitMQDeliveryMode.NonDurable;
                    clientConfiguration.RouteKey = builder.Configuration["Logging:Serilog:RabbitMq:ROUTEKEY"];
                    clientConfiguration.Port = 5672;

                    var hosts = builder.Configuration.GetSection("Logging:Serilog:RabbitMq:HOSTNAMES").Get<List<string>>();
                    foreach (string hostname in hosts)
                    {
                        clientConfiguration.Hostnames.Add(hostname);
                    }

                    sinkConfiguration.TextFormatter = new JsonFormatter();
                })
                .Enrich.WithProperty("ApplicationName", AppName)
                .CreateLogger();

            builder.Host.UseSerilog();
        }
        public static void ConfigureDapr(this WebApplicationBuilder services)
        {

            var pubSubName = services.Configuration.GetValue<string>("DaprSettings:PubSubName");

            if (pubSubName != null)
            {
                services.Services.AddScoped<IEventBus>(x =>
                                new DaprEventBus(x.GetRequiredService<DaprClient>(),
                                x.GetRequiredService<ILogger<DaprEventBus>>(),
                                pubSubName));
            }

            services.Services.AddScoped<IDaprStateStore>(sp => new DaprStateStore(sp.GetRequiredService<ILogger<DaprStateStore>>()));

        }
        public static void AddCustomApplicationServices(this WebApplicationBuilder builder)
        {
            var assemblies = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll", SearchOption.TopDirectoryOnly)
               .Where(filePath => Path.GetFileName(filePath).StartsWith("Payment"))
               .Select(Assembly.LoadFrom);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(assemblies.ToArray());
            });
        }

    }
}
