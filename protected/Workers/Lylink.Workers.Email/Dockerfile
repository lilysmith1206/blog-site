FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY . .

RUN dotnet restore protected/Workers/Lylink.Workers.Email/Lylink.Workers.Email.csproj
RUN dotnet publish protected/Workers/Lylink.Workers.Email/Lylink.Workers.Email.csproj --no-restore -o out && \
    rm -f /app/out/appsettings.json && \
    rm -f /app/out/appsettings.Development.json

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
RUN mv appsettings.Docker.json appsettings.json
EXPOSE 8080
ENTRYPOINT ["dotnet", "Lylink.Workers.Email.dll"]