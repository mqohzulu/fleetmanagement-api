using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fleet.Messaging.Abstractions;
using Fleet.Messaging.InMemory;
using Fleet.Messaging.RabbitMq;
using Identity.Application.Interfaces;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Repositories;

namespace Identity.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            var defaultConn = configuration.GetConnectionString("Default");
            if (!string.IsNullOrWhiteSpace(defaultConn))
            {
                if (defaultConn.Contains("Host=", System.StringComparison.OrdinalIgnoreCase))
                {
                    services.AddDbContext<IdentityDbContext>(options => options.UseNpgsql(defaultConn));
                }
                else if (defaultConn.Contains("Server=", System.StringComparison.OrdinalIgnoreCase))
                {
                    services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(defaultConn));
                }
                else
                {
                    services.AddDbContext<IdentityDbContext>(options => options.UseSqlite(defaultConn));
                }
            }
            else
            {
                services.AddDbContext<IdentityDbContext>(options => options.UseSqlite("Data Source=identity.db"));
            }

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, Identity.Infrastructure.Services.UserService>();
            services.AddSingleton<Identity.Application.Common.Interfaces.IPasswordHasher, Identity.Infrastructure.Services.BCryptPasswordHasher>();
            services.AddSingleton<Identity.Application.Common.Interfaces.IJwtTokenService, Identity.Infrastructure.Services.JwtTokenService>();

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

            // JWT authentication configuration is intentionally left in Program.cs to keep authentication wiring visible

            services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(opts =>
            {
                opts.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            return services;
        }
    }
}
