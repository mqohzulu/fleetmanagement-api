Fleet Management API workspace

Run the full stack with Docker Compose (Postgres, RabbitMQ, migrations, services, gateway):

```powershell
docker-compose -f infra/docker-compose/docker-compose.yml up --build
```

Run RabbitMQ only (Docker):

```powershell
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

Run the Identity API locally (without containers):

```powershell
dotnet run --project services/IdentityService/src/Identity.Api/Identity.Api.csproj
```

See infra/docker-compose for compose configuration and service definitions.

