FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
COPY private/LylinkDatabase.MigrationScripts/LylinkDatabase.MigrationScripts.csproj ./LylinkDatabase.MigrationScripts/
RUN dotnet restore LylinkDatabase.MigrationScripts/LylinkDatabase.MigrationScripts.csproj
COPY private/LylinkDatabase.MigrationScripts/ ./LylinkDatabase.MigrationScripts/
RUN dotnet publish LylinkDatabase.MigrationScripts/LylinkDatabase.MigrationScripts.csproj --no-restore -o out

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "LylinkDatabase.MigrationScripts.dll"]