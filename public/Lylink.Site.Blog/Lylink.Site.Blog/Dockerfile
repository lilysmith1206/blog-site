FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY /Directory.Packages.props ./Directory.Packages.props

COPY public/Lylink.Site.Blog/Lylink.Site.Blog/Lylink.Site.Blog.csproj ./public/Lylink.Site.Blog/Lylink.Site.Blog/
COPY public/Lylink.Site.Blog/Lylink.Site.Blog.Client/Lylink.Site.Blog.Client.csproj ./public/Lylink.Site.Blog/Lylink.Site.Blog.Client/
COPY public/Lylink.Site.Shared/Lylink.Site.Shared.csproj ./public/Lylink.Site.Shared/
COPY protected/Analytics/Lylink.Analytics.Api.Client/Lylink.Analytics.Api.Client.csproj ./protected/Analytics/Lylink.Analytics.Api.Client/
COPY protected/Analytics/Lylink.Analytics.Models/Lylink.Analytics.Models.csproj ./protected/Analytics/Lylink.Analytics.Models/
COPY protected/Blog/Lylink.Blog.Api.Client/Lylink.Blog.Api.Client.csproj ./protected/Blog/Lylink.Blog.Api.Client/
COPY protected/Blog/Lylink.Blog.Shared/Lylink.Blog.Shared.csproj ./protected/Blog/Lylink.Blog.Shared/
COPY protected/Shared/Lylink.Shared.Api/Lylink.Shared.Api.csproj ./protected/Shared/Lylink.Shared.Api/
COPY protected/Shared/Lylink.Shared.Api.Client/Lylink.Shared.Api.Client.csproj ./protected/Shared/Lylink.Shared.Api.Client/
COPY private/Lylink.Shared.Models/Lylink.Shared.Models.csproj ./private/Lylink.Shared.Models/
COPY private/Lylink.Database.Context/Lylink.Database.Context.csproj ./private/Lylink.Database.Context/

RUN dotnet restore public/Lylink.Site.Blog/Lylink.Site.Blog/Lylink.Site.Blog.csproj

COPY public/Lylink.Site.Blog/Lylink.Site.Blog/ ./public/Lylink.Site.Blog/Lylink.Site.Blog/
COPY public/Lylink.Site.Blog/Lylink.Site.Blog.Client/ ./public/Lylink.Site.Blog/Lylink.Site.Blog.Client/
COPY public/Lylink.Site.Shared/ ./public/Lylink.Site.Shared/
COPY protected/Analytics/Lylink.Analytics.Api.Client/ ./protected/Analytics/Lylink.Analytics.Api.Client/
COPY protected/Analytics/Lylink.Analytics.Models/ ./protected/Analytics/Lylink.Analytics.Models/
COPY protected/Blog/Lylink.Blog.Api.Client/ ./protected/Blog/Lylink.Blog.Api.Client/
COPY protected/Blog/Lylink.Blog.Shared/ ./protected/Blog/Lylink.Blog.Shared/
COPY protected/Shared/Lylink.Shared.Api/ ./protected/Shared/Lylink.Shared.Api/
COPY protected/Shared/Lylink.Shared.Api.Client/ ./protected/Shared/Lylink.Shared.Api.Client/
COPY private/Lylink.Shared.Models/ ./private/Lylink.Shared.Models/
COPY private/Lylink.Database.Context/ ./private/Lylink.Database.Context/

RUN dotnet publish public/Lylink.Site.Blog/Lylink.Site.Blog/Lylink.Site.Blog.csproj --no-restore -o out && \
    rm /app/out/appsettings.json && \
    rm /app/out/appsettings.Development.json

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
RUN mv appsettings.Docker.json appsettings.json
EXPOSE 8080
ENTRYPOINT ["dotnet", "Lylink.Site.Blog.dll"]