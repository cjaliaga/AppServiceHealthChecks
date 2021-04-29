using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace HealthChecks
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var instanceId = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID");

            if (string.IsNullOrEmpty(instanceId))
            {
                services.AddSingleton<IApplicationStatus, InMemoryApplicationStatus>();
            } 
            else
            {
                services.AddSingleton<IApplicationStatus, InFileApplicationStatus>();
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });

                endpoints.MapGet("healthcheck", async context =>
                {
                    var status = context.RequestServices.GetRequiredService<IApplicationStatus>();
                    var instanceId = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID");
                    var machineName = Environment.GetEnvironmentVariable("COMPUTERNAME");

                    if (await status.IsHealthyAsync())
                    {
                        context.Response.StatusCode = 200;
                        await context.Response.WriteAsync($"The instance with id '{instanceId}' and machine name '{machineName}' is healthy.");
                    } 
                    else
                    {
                        context.Response.StatusCode = 503;
                        await context.Response.WriteAsync($"The instance with id '{instanceId}' and machine name '{machineName}' is unhealthy.");
                    }
                });

                endpoints.MapGet("disable", async context =>
                {
                    var status = context.RequestServices.GetRequiredService<IApplicationStatus>();

                    await status.SetUnhealthyAsync();

                    var instanceId = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID");
                    var machineName = Environment.GetEnvironmentVariable("COMPUTERNAME");

                    await context.Response.WriteAsync($"The instance with id '{instanceId}' and machine name '{machineName}' is now unhealthy.");
                });

                endpoints.MapGet("enable", async context =>
                {
                    var status = context.RequestServices.GetRequiredService<IApplicationStatus>();

                    await status.SetHealthyAsync();

                    var instanceId = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID");
                    var machineName = Environment.GetEnvironmentVariable("COMPUTERNAME");

                    await context.Response.WriteAsync($"The instance with id '{instanceId}' and machine name '{machineName}' is now healthy.");
                });
            });
        }
    }
}
