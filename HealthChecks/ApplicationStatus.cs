using System.Threading.Tasks;

namespace HealthChecks
{
    internal abstract class ApplicationStatus
    {
        public abstract Task<bool> IsHealthyAsync();

        internal abstract Task SetUnhealthyAsync();

        internal abstract Task SetHealthyAsync();

    }
}