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

# Use the official ASP.NET Core runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Expose port (change if your app uses a different port)
EXPOSE 80
EXPOSE 443

# Set environment variables (optional, for production best practices)
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application (migrations will be applied automatically on startup via Program.cs)
ENTRYPOINT ["dotnet", "Shortha.dll"]