using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fleet.Messaging.Abstractions;
using Fleet.Messaging.InMemory;
using Fleet.Messaging.RabbitMq;
using Maintenance.Application.Interfaces;
using Maintenance.Infrastructure.Persistence;
using Maintenance.Infrastructure.Repositories;

namespace Maintenance.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMaintenanceApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            var maintConn = configuration.GetConnectionString("Default");
            if (!string.IsNullOrWhiteSpace(maintConn))
            {
                if (maintConn.Contains("Host=", System.StringComparison.OrdinalIgnoreCase))
                {
                    services.AddDbContext<MaintenanceDbContext>(options => options.UseNpgsql(maintConn));
                }
                else if (maintConn.Contains("Server=", System.StringComparison.OrdinalIgnoreCase))
                {
                    services.AddDbContext<MaintenanceDbContext>(options => options.UseSqlServer(maintConn));
                }
                else
                {
                    services.AddDbContext<MaintenanceDbContext>(options => options.UseSqlite(maintConn));
                }
            }
            else
            {
                services.AddDbContext<MaintenanceDbContext>(options => options.UseSqlite("Data Source=maintenance.db"));
            }

            services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();

            var mqSection = configuration.GetSection("RabbitMq");
            if (!string.IsNullOrWhiteSpace(mqSection["HostName"]))
            {
                var port = int.TryParse(mqSection["Port"], out var p) ? p : 5672;
                var options = new RabbitMqOptions(
                    mqSection["HostName"],
                    port,
                    mqSection["UserName"] ?? "guest",
                    mqSection["Password"] ?? "guest",
                    mqSection["ExchangeName"] ?? "fleet.events");
                services.AddSingleton<IEventBus>(_ => new RabbitMqEventBus(options));
            }
            else
            {
                services.AddSingleton<IEventBus, InMemoryEventBus>();
            }

            services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(opts =>
            {
                opts.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            return services;
        }
    }
}
