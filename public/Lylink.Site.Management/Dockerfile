FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY public/Lylink.Site.Management/Lylink.Site.Management.csproj ./public/Lylink.Site.Management/
COPY protected/Management/Lylink.Management.Api.Client/Lylink.Management.Api.Client.csproj ./protected/Management/Lylink.Management.Api.Client/
COPY protected/Management/Lylink.Management.Shared/Lylink.Management.Shared.csproj ./protected/Management/Lylink.Management.Shared/
COPY protected/Shared/Lylink.Shared.Api/Lylink.Shared.Api.csproj ./protected/Shared/Lylink.Shared.Api/
COPY protected/Shared/Lylink.Shared.Api.Client/Lylink.Shared.Api.Client.csproj ./protected/Shared/Lylink.Shared.Api.Client/
COPY private/Lylink.Shared.Models/Lylink.Shared.Models.csproj ./private/Lylink.Shared.Models/

RUN dotnet restore public/Lylink.Site.Management/Lylink.Site.Management.csproj

COPY public/Lylink.Site.Management/ ./public/Lylink.Site.Management/
COPY protected/Management/Lylink.Management.Api.Client/ ./protected/Management/Lylink.Management.Api.Client/
COPY protected/Management/Lylink.Management.Shared/ ./protected/Management/Lylink.Management.Shared/
COPY protected/Shared/Lylink.Shared.Api/ ./protected/Shared/Lylink.Shared.Api/
COPY protected/Shared/Lylink.Shared.Api.Client/ ./protected/Shared/Lylink.Shared.Api.Client/
COPY private/Lylink.Shared.Models/ ./private/Lylink.Shared.Models/

RUN dotnet publish public/Lylink.Site.Management/Lylink.Site.Management.csproj --no-restore -o out && \
    rm /app/out/appsettings.json && \
    rm /app/out/appsettings.Development.json

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
RUN mv appsettings.Docker.json appsettings.json
EXPOSE 8080
ENTRYPOINT ["dotnet", "Lylink.Site.Management.dll"]