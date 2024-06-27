using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;

namespace webapi.Services;

public class AggregateService : BackgroundService
{
    private int executionCount = 0;
    private readonly ILogger<AggregateService> _logger;
    private Timer? _timer = null;

    private IServiceScopeFactory _factory;

    public AggregateService(ILogger<AggregateService> logger, IServiceProvider services, IServiceScopeFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {

        _logger.LogInformation("Timed Hosted Service running.");

        _timer = new Timer(Aggregate, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(10));

        return Task.CompletedTask;
    }



    private async void Aggregate(object? state)
    {
        var count = Interlocked.Increment(ref executionCount);

        _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", count);
        using (var scope = _factory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<webapiContext>();
            
            
            
            var deviceQuery = context.CurrentDevices
                .Include(x => x.DeviceType)
                .GroupBy(x => new { x.DeviceType.DeviceTypeId, x.Location, x.Grade })
                .Select(x => 
                    new { x.Key.Location, x.Key.DeviceTypeId, x.Key.Grade, Count = x.Count() }
                );

                
            foreach (var device in deviceQuery)
            {
                context.AggregatedDevices.Add(
                    new AggregatedDevice
                    {
                        DeviceTypeId = device.DeviceTypeId,
                        Count = device.Count,
                        Location = device.Location,
                        Grade = device.Grade,
                        TimeStamp = DateTime.Today
                    }
                );
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogInformation("Error saving changes" + e.Message);
            }
        }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}