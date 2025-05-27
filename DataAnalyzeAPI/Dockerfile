# Production Dockerfile для DataAnalyzeAPI
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["DataAnalyzeApi.csproj", "./"]
RUN dotnet restore "./DataAnalyzeApi.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "./DataAnalyzeApi.csproj" \
	-c $BUILD_CONFIGURATION \
	-o /app/publish \
	/p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

# Create app user
RUN groupadd -r appgroup && useradd -r -g appgroup appuser

WORKDIR /app
COPY --from=build /app/publish .

# Set ownership
RUN chown -R appuser:appgroup /app
USER appuser

EXPOSE 8080

ENTRYPOINT ["dotnet", "DataAnalyzeApi.dll"]
