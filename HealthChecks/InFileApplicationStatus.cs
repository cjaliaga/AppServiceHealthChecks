using System;
using System.IO;
using System.Threading.Tasks;

namespace HealthChecks
{
    public class InFileApplicationStatus : IApplicationStatus
    {
        public Task<bool> IsHealthyAsync()
        {
            try
            {
                var instanceId = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID");
                var status = File.ReadAllText($"/home/site/wwwroot/{instanceId}.txt");
            
                if(status == "0")
                {
                    return Task.FromResult(false);
                }
            }
            catch (Exception)
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(true);
        }

        public async Task SetHealthyAsync()
        {
            var instanceId = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID");
            using var writter = File.CreateText($"/home/site/wwwroot/{instanceId}.txt");
            await writter.WriteAsync("1");
            await writter.FlushAsync();
        }

        public async Task SetUnhealthyAsync()
        {
            var instanceId = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID");
            using var writter = File.CreateText($"/home/site/wwwroot/{instanceId}.txt");
            await writter.WriteAsync("0");
            await writter.FlushAsync();
        }
    }
}
