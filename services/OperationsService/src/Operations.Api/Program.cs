using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Operations.Infrastructure.Persistence;
using Operations.Api;
using Operations.Application.Interfaces;
using Operations.Infrastructure.Repositories;
using Fleet.Messaging.InMemory;
using Fleet.Messaging.RabbitMq;
using Fleet.Messaging.Abstractions;
using Identity.Application.Events;
using Operations.Api.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var opsConn = builder.Configuration.GetConnectionString("Default");
if (!string.IsNullOrWhiteSpace(opsConn))
{
	if (opsConn.Contains("Host=", StringComparison.OrdinalIgnoreCase))
	{
		builder.Services.AddDbContext<OperationsDbContext>(options => options.UseNpgsql(opsConn));
	}
	else if (opsConn.Contains("Server=", StringComparison.OrdinalIgnoreCase))
	{
		builder.Services.AddDbContext<OperationsDbContext>(options => options.UseSqlServer(opsConn));
	}
	else
	{
		builder.Services.AddDbContext<OperationsDbContext>(options => options.UseSqlite(opsConn));
	}
}
else
{
	builder.Services.AddDbContext<OperationsDbContext>(options => options.UseSqlite("Data Source=operations.db"));
}

builder.Services.AddScoped<ITripRepository, TripRepository>();

// Messaging registration
var mqSection = builder.Configuration.GetSection("RabbitMq");
Fleet.Messaging.RabbitMq.RabbitMqOptions? rabbitOptions = null;
if (!string.IsNullOrWhiteSpace(mqSection["HostName"]))
{
	var port = int.TryParse(mqSection["Port"], out var p) ? p : 5672;
	rabbitOptions = new RabbitMqOptions(
		mqSection["HostName"],
		port,
		mqSection["UserName"] ?? "guest",
		mqSection["Password"] ?? "guest",
		mqSection["ExchangeName"] ?? "fleet.events");
	builder.Services.AddSingleton<IEventBus>(_ => new RabbitMqEventBus(rabbitOptions));
}
else
{
	builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();
}

// Centralized DI registrations
builder.Services.AddOperationsApiServices(builder.Configuration);

var app = builder.Build();

// Populate HttpContext.User from gateway-forwarded headers when present
app.Use(async (ctx, next) =>
{
	if (ctx.Request.Headers.TryGetValue("X-User-Id", out var userId))
	{
		var claims = new List<System.Security.Claims.Claim>
		{
			new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, (string)userId)
		};
		if (ctx.Request.Headers.TryGetValue("X-User-Email", out var email)) claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, (string)email));
		if (ctx.Request.Headers.TryGetValue("X-User-Name", out var name)) claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, (string)name));
		var id = new System.Security.Claims.ClaimsIdentity(claims, "Gateway");
		ctx.User = new System.Security.Claims.ClaimsPrincipal(id);
	}
	await next();
});

var bus = app.Services.GetRequiredService<IEventBus>();
if (bus is InMemoryEventBus inMemoryBus)
{
	inMemoryBus.Subscribe<UserCreatedEvent>(UserCreatedHandler.HandleAsync);
}

// If RabbitMQ is configured, start a RabbitMqSubscriber to receive cross-process events
Fleet.Messaging.RabbitMq.RabbitMqSubscriber<UserCreatedEvent>? userCreatedSubscriber = null;
if (rabbitOptions is not null)
{
	userCreatedSubscriber = new Fleet.Messaging.RabbitMq.RabbitMqSubscriber<UserCreatedEvent>(
		rabbitOptions,
		"user.created",
		UserCreatedHandler.HandleAsync);
	userCreatedSubscriber.Start();
	var subscriberRef = userCreatedSubscriber;
	app.Lifetime.ApplicationStopping.Register(() => subscriberRef?.Stop());
}

app.MapControllers();

app.Run();
