FROM mcr.microsoft.com/dotnet/sdk:7.0

WORKDIR /src

# copy repository
COPY . .

# ensure dotnet-ef is available
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool install --global dotnet-ef --version 7.* \
    && dotnet restore fleetmanagement-api.sln

# Install postgres client for DB creation when using Postgres
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

COPY docker/migrations/entrypoint.sh /scripts/entrypoint.sh
RUN chmod +x /scripts/entrypoint.sh

ENTRYPOINT ["/bin/bash", "/scripts/entrypoint.sh"]
