using ApiPulseHQ.Api.Services.Monitoring;

namespace ApiPulseHQ.Api.Workers
{
    public class MonitoringWorker : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<MonitoringWorker> _logger;

        public MonitoringWorker(IServiceProvider provider, ILogger<MonitoringWorker> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Monitoring worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _provider.CreateScope();
                var monitoringService = scope.ServiceProvider.GetRequiredService<IMonitoringService>();

                try
                {
                    await monitoringService.CheckAllAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during monitoring cycle");
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
