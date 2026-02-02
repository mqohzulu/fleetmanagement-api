using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Maintenance.Infrastructure.Persistence;
using Maintenance.Api;
using Maintenance.Application.Interfaces;
using Maintenance.Infrastructure.Repositories;
using Fleet.Messaging.InMemory;
using Fleet.Messaging.RabbitMq;
using Fleet.Messaging.Abstractions;
using Identity.Application.Events;
using Maintenance.Api.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Centralized DI registrations
builder.Services.AddMaintenanceApiServices(builder.Configuration);

var app = builder.Build();
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

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
