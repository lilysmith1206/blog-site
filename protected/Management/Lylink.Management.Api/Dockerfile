FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY /Directory.Packages.props ./Directory.Packages.props

COPY protected/Management/Lylink.Management.Api/Lylink.Management.Api.csproj ./protected/Management/Lylink.Management.Api/
COPY protected/Management/Lylink.Management.Shared/Lylink.Management.Shared.csproj ./protected/Management/Lylink.Management.Shared/
COPY protected/Shared/Lylink.Shared.Api/Lylink.Shared.Api.csproj ./protected/Shared/Lylink.Shared.Api/
COPY private/Lylink.Database.Repositories/Lylink.Database.Repositories.csproj ./private/Lylink.Database.Repositories/
COPY private/Lylink.Database.Context/Lylink.Database.Context.csproj ./private/Lylink.Database.Context/
COPY private/Lylink.Shared.Models/Lylink.Shared.Models.csproj ./private/Lylink.Shared.Models/

RUN dotnet restore protected/Management/Lylink.Management.Api/Lylink.Management.Api.csproj

COPY protected/Management/Lylink.Management.Api/ ./protected/Management/Lylink.Management.Api/
COPY protected/Management/Lylink.Management.Shared/ ./protected/Management/Lylink.Management.Shared/
COPY protected/Shared/Lylink.Shared.Api/ ./protected/Shared/Lylink.Shared.Api/
COPY private/Lylink.Database.Repositories/ ./private/Lylink.Database.Repositories/
COPY private/Lylink.Database.Context/ ./private/Lylink.Database.Context/
COPY private/Lylink.Shared.Models/ ./private/Lylink.Shared.Models/

RUN dotnet publish protected/Management/Lylink.Management.Api/Lylink.Management.Api.csproj --no-restore -o out && \
    rm /app/out/appsettings.json && \
    rm /app/out/appsettings.Development.json

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
RUN mv appsettings.Docker.json appsettings.json
EXPOSE 8080
ENTRYPOINT ["dotnet", "Lylink.Management.Api.dll"]