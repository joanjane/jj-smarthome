FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /repo
COPY ["src/JJ.SmartHome.Job/JJ.SmartHome.Job.csproj", "src/JJ.SmartHome.Job/"]
COPY ["src/JJ.SmartHome.Core/JJ.SmartHome.Core.csproj", "src/JJ.SmartHome.Core/"]
COPY ["src/JJ.SmartHome.Db/JJ.SmartHome.Db.csproj", "src/JJ.SmartHome.Db/"]
COPY ["test/JJ.SmartHome.Tests/JJ.SmartHome.Tests.csproj", "test/JJ.SmartHome.Tests/"]
COPY ["JJ.SmartHome.sln", "./"]
RUN dotnet restore

COPY . .
WORKDIR "/repo/src/JJ.SmartHome.Job"
RUN dotnet build "JJ.SmartHome.Job.csproj" -c Release -o /app/build

FROM build AS test
WORKDIR "/repo/test/JJ.SmartHome.Tests"

FROM test AS publish
WORKDIR "/repo/src/JJ.SmartHome.Job"
RUN dotnet publish "JJ.SmartHome.Job.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JJ.SmartHome.Job.dll"]