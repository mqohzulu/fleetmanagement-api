using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fleet.Messaging.Abstractions;
using Fleet.Messaging.InMemory;
using Fleet.Messaging.RabbitMq;
using Operations.Application.Interfaces;
using Operations.Infrastructure.Persistence;
using Operations.Infrastructure.Repositories;

namespace Operations.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOperationsApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            var opsConn = configuration.GetConnectionString("Default");
            if (!string.IsNullOrWhiteSpace(opsConn))
            {
                if (opsConn.Contains("Host=", System.StringComparison.OrdinalIgnoreCase))
                {
                    services.AddDbContext<OperationsDbContext>(options => options.UseNpgsql(opsConn));
                }
                else if (opsConn.Contains("Server=", System.StringComparison.OrdinalIgnoreCase))
                {
                    services.AddDbContext<OperationsDbContext>(options => options.UseSqlServer(opsConn));
                }
                else
                {
                    services.AddDbContext<OperationsDbContext>(options => options.UseSqlite(opsConn));
                }
            }
            else
            {
                services.AddDbContext<OperationsDbContext>(options => options.UseSqlite("Data Source=operations.db"));
            }

            services.AddScoped<ITripRepository, TripRepository>();

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
