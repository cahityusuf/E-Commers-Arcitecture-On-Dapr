using Dapr.Client;
using ECommers.Dapr;
using ECommers.Dapr.Abstractions;
using ECommers.Data;
using Microsoft.EntityFrameworkCore;
using Order.Application.Actors;
using Order.Domain.AggregatesModel.AddressAggregate;
using Order.Domain.AggregatesModel.OrderAggregate;
using Order.Domain.AggregatesModel.OrderItemAggregate;
using Order.Infrastructure.DbContexts;
using Order.Infrastructure.Repositories;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ;
using System.Reflection;

namespace Order.API.Helpers
{
    public static class StartupExtensions
    {
        private const string AppName = "Order API";
        public static WebApplicationBuilder AddOrderApi(this WebApplicationBuilder builder)
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
               .Where(filePath => Path.GetFileName(filePath).StartsWith("Order"))
               .Select(Assembly.LoadFrom);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(assemblies.ToArray());
            });

            builder.Services.AddDbContext<OrderDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SampleConnectionstring"));
            });

            builder.Services.AddActors(options =>
            {
                options.Actors.RegisterActor<OrderingProcessActor>();
            });

            builder.Services.AddAutoMapper(assemblies);
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));
            builder.Services.AddScoped(typeof(IAddressRepository), typeof(AddressRepository));
            builder.Services.AddScoped(typeof(IOrderItemRepository), typeof(OrderItemRepository));

        }

    }
}
