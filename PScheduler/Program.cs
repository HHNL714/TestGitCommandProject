using PScheduler.ExtentionClass;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

#region Add For Scheduler

var isDebugging = !(Debugger.IsAttached || args.Contains("--console"));
var hostBuilder = new HostBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<BackupService>();
    });
if (isDebugging)
{
    await hostBuilder.RunTheServiceAsync();
}
else
{
    await hostBuilder.RunConsoleAsync();
}

#endregion

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
