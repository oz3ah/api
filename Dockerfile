FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file, all relevant project files first to leverage Docker layer caching for restore
COPY ["Shortha.sln", "./"]

# Copy main project file for the application
COPY ["Shortha/Shortha.csproj", "Shortha/"]

# Copy referenced project files
COPY ["Shortha.Domain/Shortha.Domain.csproj", "Shortha.Domain/"]
COPY ["Shortha.Application/Shortha.Application.csproj", "Shortha.Application/"]
COPY ["Shortha.Infrastructre/Shortha.Infrastructre.csproj", "Shortha.Infrastructre/"]

# Restore dependencies using the solution file
RUN dotnet restore "Shortha.sln"

# Copy the rest of the source code
# This copies everything from the build context (e.g., ShorthaProject/) into /src
# So, Shortha/ code goes to /src/Shortha/, Payment/ to /src/Payment/, etc.
COPY . .

# Build and publish the app
# WORKDIR is /src, so paths should be relative to /src
RUN dotnet publish "Shortha/Shortha.csproj" -c Release -o /app/publish --no-restore

# Create a migration script for runtime
RUN echo '#!/bin/bash\n\
    set -e\n\
    echo "Waiting for database to be ready..."\n\
    sleep 10\n\
    echo "Applying database migrations..."\n\
    dotnet ef database update --project "Shortha.Infrastructre" --startup-project "Shortha" --configuration Release --no-build\n\
    echo "Migrations completed successfully"\n\
    exec "$@"' > /app/migrate-and-run.sh && chmod +x /app/migrate-and-run.sh

# Use the official ASP.NET Core SDK image for the final stage (needed for EF tools)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app

# Install EF Core tools for migrations
RUN dotnet tool install --global dotnet-ef --version 9.0.7

# Copy published output
COPY --from=build /app/publish .

# Copy the migration script
COPY --from=build /app/migrate-and-run.sh .

# Copy the source code for EF migrations (needed for runtime migrations)
COPY --from=build /src/Shortha.Infrastructre ./Shortha.Infrastructre
COPY --from=build /src/Shortha ./Shortha

# Expose port (change if your app uses a different port)
EXPOSE 80
EXPOSE 443

# Set environment variables (optional, for production best practices)
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Run migrations and then the application
ENTRYPOINT ["./migrate-and-run.sh", "dotnet", "Shortha.dll"]