FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY /Directory.Packages.props ./Directory.Packages.props

COPY protected/Workers/Lylink.Workers.Email/Lylink.Workers.Email.csproj ./protected/Workers/Lylink.Workers.Email/
COPY private/Lylink.Database.Repositories/Lylink.Database.Repositories.csproj ./private/Lylink.Database.Repositories/
COPY private/Lylink.Database.Context/Lylink.Database.Context.csproj ./private/Lylink.Database.Context/
COPY private/Lylink.Shared.Models/Lylink.Shared.Models.csproj ./private/Lylink.Shared.Models/

RUN dotnet restore protected/Workers/Lylink.Workers.Email/Lylink.Workers.Email.csproj

COPY protected/Workers/Lylink.Workers.Email/ ./protected/Workers/Lylink.Workers.Email/
COPY private/Lylink.Database.Repositories/ ./private/Lylink.Database.Repositories/
COPY private/Lylink.Database.Context/ ./private/Lylink.Database.Context/
COPY private/Lylink.Shared.Models/ ./private/Lylink.Shared.Models/

RUN dotnet publish protected/Workers/Lylink.Workers.Email/Lylink.Workers.Email.csproj --no-restore -o out && \
    rm /app/out/appsettings.json && \
    rm /app/out/appsettings.Development.json

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
RUN mv appsettings.Docker.json appsettings.json
EXPOSE 8080
ENTRYPOINT ["dotnet", "Lylink.Workers.Email.dll"]