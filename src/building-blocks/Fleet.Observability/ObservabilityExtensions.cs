using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fleet.Observability
{
    public static class ObservabilityExtensions
    {
        public static IServiceCollection AddObservability(this IServiceCollection services)
        {
            services.AddHealthChecks();
            // Additional observability integrations (OpenTelemetry, metrics, logging) can be added here.
            return services;
        }

        public static IApplicationBuilder UseObservability(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health");
            return app;
        }
    }
}
