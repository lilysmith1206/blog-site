FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY protected/Analytics/LylinkBackend.AnalyticsApi/LylinkBackend.AnalyticsApi.csproj ./protected/Analytics/LylinkBackend.AnalyticsApi/
COPY protected/Shared/LylinkBackend.SharedApiCode/LylinkBackend.SharedApiCode.csproj ./protected/Shared/LylinkBackend.SharedApiCode/
COPY private/LylinkBackend.Repositories/LylinkBackend.Repositories.csproj ./private/LylinkBackend.Repositories/
COPY private/LylinkBackend_DatabaseAccessLayer/LylinkBackend_DatabaseAccessLayer.csproj ./private/LylinkBackend_DatabaseAccessLayer/

RUN dotnet restore protected/Analytics/LylinkBackend.AnalyticsApi/LylinkBackend.AnalyticsApi.csproj

COPY protected/Analytics/LylinkBackend.AnalyticsApi/ ./protected/Analytics/LylinkBackend.AnalyticsApi/
COPY protected/Shared/LylinkBackend.SharedApiCode/ ./protected/Shared/LylinkBackend.SharedApiCode/
COPY private/LylinkBackend.Repositories/ ./private/LylinkBackend.Repositories/
COPY private/LylinkBackend_DatabaseAccessLayer/ ./private/LylinkBackend_DatabaseAccessLayer/

RUN dotnet publish protected/Analytics/LylinkBackend.AnalyticsApi/LylinkBackend.AnalyticsApi.csproj --no-restore -o out

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "LylinkBackend.AnalyticsApi.dll"]