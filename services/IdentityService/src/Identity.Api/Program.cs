using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Fleet.Messaging.InMemory;
using Fleet.Messaging.RabbitMq;
using Fleet.Messaging.Abstractions;
using Identity.Infrastructure.Persistence;
using Identity.Api;
using Identity.Application.Interfaces;
using Identity.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Centralized DI registrations
builder.Services.AddIdentityApiServices(builder.Configuration);

// JWT Authentication is kept inline for clarity
var jwtSection = builder.Configuration.GetSection("Jwt");
var key = jwtSection["Key"] ?? "change_this_secret_in_prod";
var issuer = jwtSection["Issuer"] ?? "fleet";
var audience = jwtSection["Audience"] ?? "fleet_clients";

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
	.AddJwtBearer(options =>
	{
		options.RequireHttpsMetadata = false;
		options.SaveToken = true;
		options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = issuer,
			ValidAudience = audience,
			IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key))
		};
	});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();