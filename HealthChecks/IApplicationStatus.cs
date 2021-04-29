using System.Threading.Tasks;

namespace HealthChecks
{
    public interface IApplicationStatus
    {
        public Task<bool> IsHealthyAsync();

        public Task SetUnhealthyAsync();

        public Task SetHealthyAsync();
    }
}