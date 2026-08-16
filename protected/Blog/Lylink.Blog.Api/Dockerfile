FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY . .

RUN dotnet restore protected/Blog/Lylink.Blog.Api/Lylink.Blog.Api.csproj
RUN dotnet publish protected/Blog/Lylink.Blog.Api/Lylink.Blog.Api.csproj --no-restore -o out && \
    rm -f /app/out/appsettings.json && \
    rm -f /app/out/appsettings.Development.json

FROM ghcr.io/lilysmith1206/aspnet-with-health-check:latest AS runtime
WORKDIR /app
COPY --from=build /app/out .
RUN mv appsettings.Docker.json appsettings.json
EXPOSE 8080
ENTRYPOINT ["dotnet", "Lylink.Blog.Api.dll"]