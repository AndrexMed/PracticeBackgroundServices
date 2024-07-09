using AutomaticProcess.Interfaces;
using AutomaticProcess.Options;
using Microsoft.Extensions.Options;
using ProcesosAutomaticos.Api.BackgroundServices;

namespace AutomaticProcess.BackgroundServices
{
    public class TestOneBackgroundService : CronoJobService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<TestOneBackgroundService> _logger;

        public TestOneBackgroundService(IServiceScopeFactory serviceScopeFactory,
                                        ILogger<TestOneBackgroundService> logger,
                                        IOptions<TestOptions> options) : base(options.Value.TestProgramming, TimeZoneInfo.Local)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The process automatic has been initializated.");
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var testService = scope.ServiceProvider.GetRequiredService<ITestOneService>();
                if (testService != null)
                {
                    await testService.DisabledState();
                }
            }

            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} The process automatic is running");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The service has stopped.");
            return base.StopAsync(cancellationToken);
        }
    }
}
