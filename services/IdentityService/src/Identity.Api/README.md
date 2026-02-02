Identity Service — local dev

Run RabbitMQ (Docker):

```powershell
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

Run the API (from repo root):

```powershell
dotnet run --project services/IdentityService/src/Identity.Api/Identity.Api.csproj
```

Notes:
- `appsettings.Development.json` contains the `RabbitMq` section and a local SQLite fallback.
- The service will use `InMemoryEventBus` if RabbitMQ settings are not reachable; ensure Docker RabbitMQ is running for `RabbitMqEventBus` to be used.
