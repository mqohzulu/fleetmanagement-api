using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fleet.Messaging.Abstractions;
using Fleet.Messaging.InMemory;
using Fleet.Messaging.RabbitMq;
using Fleet.Application.Interfaces;
using Fleet.Infrastructure.Persistence;
using Fleet.Infrastructure.Repositories;

namespace Fleet.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFleetApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            var fleetConn = configuration.GetConnectionString("Default");
            if (!string.IsNullOrWhiteSpace(fleetConn))
            {
                if (fleetConn.Contains("Host=", System.StringComparison.OrdinalIgnoreCase))
                {
                    services.AddDbContext<FleetDbContext>(options => options.UseNpgsql(fleetConn));
                }
                else if (fleetConn.Contains("Server=", System.StringComparison.OrdinalIgnoreCase))
                {
                    services.AddDbContext<FleetDbContext>(options => options.UseSqlServer(fleetConn));
                }
                else
                {
                    services.AddDbContext<FleetDbContext>(options => options.UseSqlite(fleetConn));
                }
            }
            else
            {
                services.AddDbContext<FleetDbContext>(options => options.UseSqlite("Data Source=fleet.db"));
            }

            services.AddScoped<IVehicleRepository, VehicleRepository>();

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
