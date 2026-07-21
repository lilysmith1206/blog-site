FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY protected/Blog/Lylink.Blog.Api/Lylink.Blog.Api.csproj ./protected/Blog/Lylink.Blog.Api/
COPY protected/Blog/Lylink.Blog.Shared/Lylink.Blog.Shared.csproj ./protected/Blog/Lylink.Blog.Shared/
COPY protected/Shared/Lylink.Shared.Api/Lylink.Shared.Api.csproj ./protected/Shared/Lylink.Shared.Api/
COPY private/Lylink.Database.Repositories/Lylink.Database.Repositories.csproj ./private/Lylink.Database.Repositories/
COPY private/Lylink.Database.Context/Lylink.Database.Context.csproj ./private/Lylink.Database.Context/
COPY private/Lylink.Shared.Models/Lylink.Shared.Models.csproj ./private/Lylink.Shared.Models/

RUN dotnet restore protected/Blog/Lylink.Blog.Api/Lylink.Blog.Api.csproj

COPY protected/Blog/Lylink.Blog.Api/ ./protected/Blog/Lylink.Blog.Api/
COPY protected/Blog/Lylink.Blog.Shared/ ./protected/Blog/Lylink.Blog.Shared/
COPY protected/Shared/Lylink.Shared.Api/ ./protected/Shared/Lylink.Shared.Api/
COPY private/Lylink.Database.Repositories/ ./private/Lylink.Database.Repositories/
COPY private/Lylink.Database.Context/ ./private/Lylink.Database.Context/
COPY private/Lylink.Shared.Models/ ./private/Lylink.Shared.Models/

RUN dotnet publish protected/Blog/Lylink.Blog.Api/Lylink.Blog.Api.csproj --no-restore -o out && \
    rm /app/out/appsettings.json && \
    rm /app/out/appsettings.Development.json

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
RUN mv appsettings.Docker.json appsettings.json
EXPOSE 8080
ENTRYPOINT ["dotnet", "Lylink.Blog.Api.dll"]