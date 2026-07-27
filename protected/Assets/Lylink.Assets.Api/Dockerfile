FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY /Directory.Packages.props ./Directory.Packages.props

COPY protected/Assets/Lylink.Assets.Api/Lylink.Assets.Api.csproj ./protected/Assets/Lylink.Assets.Api/

RUN dotnet restore protected/Assets/Lylink.Assets.Api/Lylink.Assets.Api.csproj

COPY protected/Assets/Lylink.Assets.Api/ ./protected/Assets/Lylink.Assets.Api/

RUN dotnet publish protected/Assets/Lylink.Assets.Api/Lylink.Assets.Api.csproj --no-restore -o out && \
    rm /app/out/appsettings.json && \
    rm /app/out/appsettings.Development.json

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
RUN mv appsettings.Docker.json appsettings.json
EXPOSE 8080
ENTRYPOINT ["dotnet", "Lylink.Assets.Api.dll"]