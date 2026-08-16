FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY . .

RUN dotnet restore public/Lylink.Site.Blog/Lylink.Site.Blog/Lylink.Site.Blog.csproj
RUN dotnet publish public/Lylink.Site.Blog/Lylink.Site.Blog/Lylink.Site.Blog.csproj --no-restore -o out && \
    rm -f /app/out/appsettings.json && \
    rm -f /app/out/appsettings.Development.json

FROM ghcr.io/lilysmith1206/aspnet-with-health-check:latest AS runtime
WORKDIR /app
COPY --from=build /app/out .
RUN mv appsettings.Docker.json appsettings.json
ENV HEALTH_CHECK_PATH=/api/health
EXPOSE 8080
ENTRYPOINT ["dotnet", "Lylink.Site.Blog.dll"]