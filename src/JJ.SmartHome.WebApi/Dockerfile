FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /repo
COPY ["src/JJ.SmartHome.WebApi/JJ.SmartHome.WebApi.csproj", "src/JJ.SmartHome.WebApi/"]
COPY ["src/JJ.SmartHome.Core/JJ.SmartHome.Core.csproj", "src/JJ.SmartHome.Core/"]
COPY ["src/JJ.SmartHome.Notifications/JJ.SmartHome.Notifications.csproj", "src/JJ.SmartHome.Notifications/"]
COPY ["src/JJ.SmartHome.Db/JJ.SmartHome.Db.csproj", "src/JJ.SmartHome.Db/"]
COPY ["test/JJ.SmartHome.Tests/JJ.SmartHome.Tests.csproj", "test/JJ.SmartHome.Tests/"]
COPY ["JJ.SmartHome.sln", "./"]
RUN dotnet restore src/JJ.SmartHome.WebApi/JJ.SmartHome.WebApi.csproj

COPY . .
WORKDIR "/repo/src/JJ.SmartHome.WebApi"
RUN dotnet build "JJ.SmartHome.WebApi.csproj" -c Release -o /app/build

FROM build AS test
WORKDIR "/repo/test/JJ.SmartHome.Tests"

FROM test AS publish
WORKDIR "/repo/src/JJ.SmartHome.WebApi"
RUN dotnet publish "JJ.SmartHome.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JJ.SmartHome.WebApi.dll"]