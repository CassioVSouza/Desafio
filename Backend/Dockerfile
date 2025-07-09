# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .

RUN dotnet publish -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app
COPY --from=build /app/publish .

#Força ambiente como Development para Swagger funcionar, pois por padrão o ambiente do docker não é considerado development
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "Backend.dll"]
