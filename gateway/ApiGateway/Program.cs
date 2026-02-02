using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure JWT validation for gateway
var jwtSection = builder.Configuration.GetSection("Jwt");
var key = jwtSection["Key"] ?? "change_this_secret_in_dev";
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
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = issuer,
			ValidAudience = audience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
		};
	});

builder.Services.AddReverseProxy()
	.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
	.AddTransforms(transformBuilderContext =>
	{
		transformBuilderContext.AddRequestTransform(async transformContext =>
		{
			var ctx = transformContext.HttpContext;

			// If authenticated, forward selected claims as headers and remove Authorization
			if (ctx.User?.Identity?.IsAuthenticated == true)
			{
				var id = ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				var email = ctx.User.FindFirst(ClaimTypes.Email)?.Value;
				var name = ctx.User.FindFirst(ClaimTypes.Name)?.Value;

				transformContext.ProxyRequest.Headers.Remove("Authorization");
				if (!string.IsNullOrWhiteSpace(id)) transformContext.ProxyRequest.Headers.TryAddWithoutValidation("X-User-Id", id);
				if (!string.IsNullOrWhiteSpace(email)) transformContext.ProxyRequest.Headers.TryAddWithoutValidation("X-User-Email", email);
				if (!string.IsNullOrWhiteSpace(name)) transformContext.ProxyRequest.Headers.TryAddWithoutValidation("X-User-Name", name);
			}
			else
			{
				// Ensure Authorization is forwarded if unauthenticated (fallback)
				var req = ctx.Request;
				if (req.Headers.TryGetValue("Authorization", out var auth))
				{
					transformContext.ProxyRequest.Headers.Remove("Authorization");
					transformContext.ProxyRequest.Headers.TryAddWithoutValidation("Authorization", (string)auth);
				}
			}
			await Task.CompletedTask;
		});
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

app.MapReverseProxy().RequireAuthorization();

app.Run();
