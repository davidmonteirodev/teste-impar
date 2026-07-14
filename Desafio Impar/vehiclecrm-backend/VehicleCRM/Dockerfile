# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["src/VehicleCRM.API/VehicleCRM.API.csproj", "src/VehicleCRM.API/"]
COPY ["src/VehicleCRM.Application/VehicleCRM.Application.csproj", "src/VehicleCRM.Application/"]
COPY ["src/VehicleCRM.Domain/VehicleCRM.Domain.csproj", "src/VehicleCRM.Domain/"]
COPY ["src/VehicleCRM.Infrastructure/VehicleCRM.Infrastructure.csproj", "src/VehicleCRM.Infrastructure/"]
COPY ["src/VehicleCRM.IoC/VehicleCRM.IoC.csproj", "src/VehicleCRM.IoC/"]

# Restore dependencies
RUN dotnet restore "src/VehicleCRM.API/VehicleCRM.API.csproj"

# Copy all source files
COPY . .

# Build the application
WORKDIR "/src/src/VehicleCRM.API"
RUN dotnet build "VehicleCRM.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "VehicleCRM.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Install curl for health check
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published files from publish stage
COPY --from=publish /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "VehicleCRM.API.dll"]
