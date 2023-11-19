using Order.API.Helpers;

var appName = "Basket API";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddProductApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCloudEvents();
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapSubscribeHandler();
app.MapControllers();

try
{
    app.Logger.LogInformation("Applying database migration ({ApplicationName})...", appName);
    app.ApplyDatabaseMigration();

    app.Logger.LogInformation("Starting web host ({ApplicationName})...", appName);
    app.Run();
}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "Host terminated unexpectedly ({ApplicationName})...", appName);
}
finally
{
    Serilog.Log.CloseAndFlush();
}
