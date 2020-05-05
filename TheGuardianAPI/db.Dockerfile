FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln ./
COPY TheGuardianAPI/*.csproj TheGuardianAPI/
COPY TheGuardian.Core/*.csproj TheGuardian.Core/
COPY TheGuardian.DataAccess/*.csproj TheGuardian.DataAccess/
COPY TheGuardianAPI.Test/*.csproj TheGuardianAPI.Test/
RUN dotnet restore

COPY .config ./
RUN dotnet tool restore

# Copy everything else and generate SQL script from migrations
COPY . ./
RUN dotnet ef migrations script -p TheGuardian.DataAccess -s TheGuardianAPI -o init-db.sql

# Build runtime image
FROM postgres:12.0
WORKDIR /docker-entrypoint-initdb.d
ENV POSTGRES_PASSWORD R3vTra1n1ng
COPY --from=build-env /app/init-db.sql .