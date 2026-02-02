using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Fleet.Infrastructure.Persistence;
using Fleet.Api;
using Fleet.Application.Interfaces;
using Fleet.Infrastructure.Repositories;
using Fleet.Messaging.InMemory;
using Fleet.Messaging.RabbitMq;
using Fleet.Messaging.Abstractions;
using Identity.Application.Events;
using Fleet.Api.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Centralized DI registrations
builder.Services.AddFleetApiServices(builder.Configuration);

var fleetConn = builder.Configuration.GetConnectionString("Default");
if (!string.IsNullOrWhiteSpace(fleetConn))
{
	if (fleetConn.Contains("Host=", StringComparison.OrdinalIgnoreCase))
	{
		builder.Services.AddDbContext<FleetDbContext>(options => options.UseNpgsql(fleetConn));
	}
	else if (fleetConn.Contains("Server=", StringComparison.OrdinalIgnoreCase))
	{
		builder.Services.AddDbContext<FleetDbContext>(options => options.UseSqlServer(fleetConn));
	}
	else
	{
		builder.Services.AddDbContext<FleetDbContext>(options => options.UseSqlite(fleetConn));
	}
}
else
{
	builder.Services.AddDbContext<FleetDbContext>(options => options.UseSqlite("Data Source=fleet.db"));
}

builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

// Messaging registration: prefer RabbitMQ when configured, otherwise in-memory bus
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

// Subscribe to events when using the in-memory bus (dev)
var bus = app.Services.GetRequiredService<Fleet.Messaging.Abstractions.IEventBus>();
if (bus is Fleet.Messaging.InMemory.InMemoryEventBus inMemoryBus)
{
	inMemoryBus.Subscribe<Identity.Application.Events.UserCreatedEvent>(UserCreatedHandler.HandleAsync);
}

// If RabbitMQ is configured, start a RabbitMqSubscriber to receive cross-process events
Fleet.Messaging.RabbitMq.RabbitMqSubscriber<Identity.Application.Events.UserCreatedEvent>? userCreatedSubscriber = null;
if (rabbitOptions is not null)
{
	userCreatedSubscriber = new Fleet.Messaging.RabbitMq.RabbitMqSubscriber<Identity.Application.Events.UserCreatedEvent>(
		rabbitOptions,
		"user.created",
		UserCreatedHandler.HandleAsync);
	userCreatedSubscriber.Start();
	var subscriberRef = userCreatedSubscriber;
	app.Lifetime.ApplicationStopping.Register(() => subscriberRef?.Stop());
}

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
