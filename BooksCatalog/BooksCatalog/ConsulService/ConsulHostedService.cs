using Consul;
using DataAbstraction.Models;
using Microsoft.Extensions.Options;

namespace BooksCatalog.ConsulService
{
    public class ConsulHostedService : IHostedService
    {
        private CancellationTokenSource _cts;
        private readonly IConsulClient _consulClient;
        private readonly IOptions<ConsulConfiguration> _consulConfig;
        private readonly ILogger<ConsulHostedService> _logger;
        private string _serviceId;

        public ConsulHostedService(
            IConsulClient consulClient, 
            IOptions<ConsulConfiguration> consulConfig, 
            ILogger<ConsulHostedService> logger
            )
        {
            _logger = logger;
            _consulConfig = consulConfig;
            _consulClient = consulClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            _serviceId = _consulConfig.Value.ServiceID;

            var registration = new AgentServiceRegistration()
            {
                ID = _serviceId,
                Name = _consulConfig.Value.ServiceName,
                Address = "https://localhost",
                Port = 44313,
                Tags = new[] { "Book", "Catalog" },
                Check = new AgentServiceCheck()
                {
                    HTTP = $"https://localhost:44313/healthz",
                    Timeout = TimeSpan.FromSeconds(2),
                    Interval = TimeSpan.FromSeconds(10)
                }
            };

            _logger.LogWarning("Registering in Consul");
            await _consulClient.Agent.ServiceDeregister(registration.ID, _cts.Token);
            await _consulClient.Agent.ServiceRegister(registration, _cts.Token);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            _logger.LogWarning("Deregistering from Consul");
            try
            {
                await _consulClient.Agent.ServiceDeregister(_serviceId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Deregisteration failed");
            }
        }
    }
}
