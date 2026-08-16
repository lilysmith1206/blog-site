FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY . .

RUN dotnet restore private/Lylink.Database.Migration/Lylink.Database.Migration.csproj
RUN dotnet publish private/Lylink.Database.Migration/Lylink.Database.Migration.csproj --no-restore -o out

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Lylink.Database.Migration.dll"]