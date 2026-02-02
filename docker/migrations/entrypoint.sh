#!/usr/bin/env bash
set -euo pipefail

if [ -z "${MIGRATE_PROJECT+x}" ]; then
  echo "MIGRATE_PROJECT env var is required (path to the .csproj to run migrations for)"
  exit 1
fi

STARTUP_PROJECT=${STARTUP_PROJECT:-$MIGRATE_PROJECT}

echo "Repository root: $(pwd)"
echo "Running migrations for project: $MIGRATE_PROJECT"
echo "Using startup project: $STARTUP_PROJECT"

# Restore just in case
dotnet restore "$STARTUP_PROJECT"

# If Postgres DB details provided, wait for Postgres and create the database if missing
if [ ! -z "${DB_HOST:-}" ] && [ ! -z "${DB_NAME:-}" ] && [ ! -z "${DB_USER:-}" ]; then
  export PGPASSWORD="${DB_PASS:-}"
  echo "Waiting for Postgres at $DB_HOST:$DB_PORT..."
  until psql -h "$DB_HOST" -U "$DB_USER" -c '\l' >/dev/null 2>&1; do
    sleep 1
    echo -n '.'
  done
  echo "\nPostgres is ready. Ensuring database $DB_NAME exists..."
  # create database if it does not exist
  psql -h "$DB_HOST" -U "$DB_USER" -tc "SELECT 1 FROM pg_database WHERE datname = '$DB_NAME'" | grep -q 1 || psql -h "$DB_HOST" -U "$DB_USER" -c "CREATE DATABASE \"$DB_NAME\""
fi

# Apply migrations
dotnet ef database update --project "$MIGRATE_PROJECT" --startup-project "$STARTUP_PROJECT"

echo "Migrations applied."
