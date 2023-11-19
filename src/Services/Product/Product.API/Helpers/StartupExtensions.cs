﻿using Dapr.Client;
using ECommers.Dapr;
using ECommers.Dapr.Abstractions;
using ECommers.Data;
using Microsoft.EntityFrameworkCore;
using Product.Domain.AggregatesModel.CatalogBrandAggregate;
using Product.Domain.AggregatesModel.CatalogItemAggregate;
using Product.Domain.AggregatesModel.CatalogTypeAggregate;
using Product.Infrastructure.DbContexts;
using Product.Infrastructure.Repositories;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ;
using System.Reflection;

namespace Order.API.Helpers
{
    public static class StartupExtensions
    {
        private const string AppName = "Product API";
        public static WebApplicationBuilder AddProductApi(this WebApplicationBuilder builder)
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
            services.Services.AddDaprClient();

            services.Services.AddScoped<IDaprStateStore>(sp => new DaprStateStore(sp.GetRequiredService<ILogger<DaprStateStore>>()));

        }
        public static void ApplyDatabaseMigration(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

            context.Database.Migrate();
        }
        public static void AddCustomApplicationServices(this WebApplicationBuilder builder)
        {
            var assemblies = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll", SearchOption.TopDirectoryOnly)
               .Where(filePath => Path.GetFileName(filePath).StartsWith("Product"))
               .Select(Assembly.LoadFrom);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(assemblies.ToArray());
            });

            builder.Services.AddDbContext<ProductDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ProductConnectionstring"),
                    b => b.MigrationsAssembly("Product.Infrastructure"));
            });

            builder.Services.AddAutoMapper(assemblies);
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped(typeof(ICatalogBrandRepository), typeof(CatalogBrandRepository));
            builder.Services.AddScoped(typeof(ICatalogItemRepository), typeof(CatalogItemRepository));
            builder.Services.AddScoped(typeof(ICatalogTypeRepository), typeof(CatalogTypeRepository));
        }

    }
}
