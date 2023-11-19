using Dapr.Client;
using ECommers.Dapr;
using ECommers.Dapr.Abstractions;
using Serilog;
using System.Reflection;
using Web.HttpAggregator.Abstraction.Services;
using Web.HttpAggregator.Application.Services;

namespace Basket.API.Helpers
{
    public static class StartupExtensions
    {
        private const string AppName = "Basket API";
        public static WebApplicationBuilder AddBasketApi(this WebApplicationBuilder builder)
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
               .Where(filePath => Path.GetFileName(filePath).StartsWith("Basket"))
               .Select(Assembly.LoadFrom);

            builder.Services.AddScoped<IBasketService, BasketService>();

        }

    }
}
