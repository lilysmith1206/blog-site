FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY /Directory.Packages.props ./Directory.Packages.props

COPY protected/Analytics/Lylink.Analytics.Api/Lylink.Analytics.Api.csproj ./protected/Analytics/Lylink.Analytics.Api/
COPY protected/Analytics/Lylink.Analytics.Models/Lylink.Analytics.Models.csproj ./protected/Analytics/Lylink.Analytics.Models/
COPY protected/Shared/Lylink.Shared.Api/Lylink.Shared.Api.csproj ./protected/Shared/Lylink.Shared.Api/
COPY private/Lylink.Database.Repositories/Lylink.Database.Repositories.csproj ./private/Lylink.Database.Repositories/
COPY private/Lylink.Database.Context/Lylink.Database.Context.csproj ./private/Lylink.Database.Context/
COPY private/Lylink.Shared.Models/Lylink.Shared.Models.csproj ./private/Lylink.Shared.Models/
COPY protected/Shared/Lylink.Shared/Lylink.Shared.csproj ./protected/Shared/Lylink.Shared/

RUN dotnet restore protected/Analytics/Lylink.Analytics.Api/Lylink.Analytics.Api.csproj

COPY protected/Analytics/Lylink.Analytics.Api/ ./protected/Analytics/Lylink.Analytics.Api/
COPY protected/Analytics/Lylink.Analytics.Models/ ./protected/Analytics/Lylink.Analytics.Models/
COPY protected/Shared/Lylink.Shared.Api/ ./protected/Shared/Lylink.Shared.Api/
COPY private/Lylink.Database.Repositories/ ./private/Lylink.Database.Repositories/
COPY private/Lylink.Database.Context/ ./private/Lylink.Database.Context/
COPY private/Lylink.Shared.Models/ ./private/Lylink.Shared.Models/
COPY protected/Shared/Lylink.Shared/ ./protected/Shared/Lylink.Shared/

RUN dotnet publish protected/Analytics/Lylink.Analytics.Api/Lylink.Analytics.Api.csproj --no-restore -o out && \
    rm /app/out/appsettings.json && \
    rm /app/out/appsettings.Development.json

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
RUN mv appsettings.Docker.json appsettings.json
EXPOSE 8080
ENTRYPOINT ["dotnet", "Lylink.Analytics.Api.dll"]