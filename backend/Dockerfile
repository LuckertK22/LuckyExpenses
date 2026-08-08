# ============================================================
# Stage 1: Build and publish the application
# ============================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0-noble AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy project file first for better caching
COPY ["LuckyExpenses.csproj", "."]

# Restore NuGet packages
RUN dotnet restore "LuckyExpenses.csproj"

# Copy the rest of the source code
COPY . .

# Publish the application
RUN dotnet publish "LuckyExpenses.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ============================================================
# Stage 2: Final runtime image
# ============================================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble AS final

WORKDIR /app

# Copy published application from build stage
COPY --from=build /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# Expose the port the application listens on
EXPOSE 8080

# Install curl for health checks
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

# Configure health check against the public health endpoint
HEALTHCHECK --interval=30s --timeout=3s --start-period=15s --retries=3 \
    CMD curl -f http://localhost:8080/api/v1/Health/Health || exit 1

# Switch to non-root user for security
USER $APP_UID

# Set the entry point for the application
ENTRYPOINT ["dotnet", "LuckyExpenses.dll"]
