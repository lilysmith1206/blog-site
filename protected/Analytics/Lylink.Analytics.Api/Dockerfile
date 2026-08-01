FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY . .

RUN dotnet restore protected/Analytics/Lylink.Analytics.Api/Lylink.Analytics.Api.csproj
RUN dotnet publish protected/Analytics/Lylink.Analytics.Api/Lylink.Analytics.Api.csproj --no-restore -o out && \
    rm /app/out/appsettings.json && \
    rm /app/out/appsettings.Development.json

FROM ghcr.io/lilysmith1206/aspnet-with-health-check:latest AS runtime
WORKDIR /app
COPY --from=build /app/out .
RUN mv appsettings.Docker.json appsettings.json
EXPOSE 8080
ENTRYPOINT ["dotnet", "Lylink.Analytics.Api.dll"]