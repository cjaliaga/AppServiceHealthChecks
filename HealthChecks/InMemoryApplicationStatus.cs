using System.Threading.Tasks;

namespace HealthChecks
{
    public class InMemoryApplicationStatus : IApplicationStatus
    {
        private bool _isHealthy = true;

        public Task<bool> IsHealthyAsync()
        {
            return Task.FromResult(_isHealthy);
        }

        public Task SetHealthyAsync()
        {
            _isHealthy = true;
            return Task.CompletedTask;
        }

        public Task SetUnhealthyAsync()
        {
            _isHealthy = false;
            return Task.CompletedTask;
        }

    }
}