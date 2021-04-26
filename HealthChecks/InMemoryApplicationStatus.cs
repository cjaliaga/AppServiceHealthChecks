using System.Threading.Tasks;

namespace HealthChecks
{
    internal class InMemoryApplicationStatus : ApplicationStatus
    {
        private bool _isHealthy = true;

        public override Task<bool> IsHealthyAsync()
        {
            return Task.FromResult(_isHealthy);
        }

        internal override Task SetHealthyAsync()
        {
            _isHealthy = true;
            return Task.CompletedTask;
        }

        internal override Task SetUnhealthyAsync()
        {
            _isHealthy = false;
            return Task.CompletedTask;
        }
    }
}