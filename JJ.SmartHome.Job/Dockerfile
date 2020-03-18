FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["JJ.SmartHome.Job/JJ.SmartHome.Job.csproj", "JJ.SmartHome.Job/"]
COPY ["JJ.SmartHome.Core/JJ.SmartHome.Core.csproj", "JJ.SmartHome.Core/"]
RUN dotnet restore "JJ.SmartHome.Job/JJ.SmartHome.Job.csproj"
COPY . .
WORKDIR "/src/JJ.SmartHome.Job"
RUN dotnet build "JJ.SmartHome.Job.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JJ.SmartHome.Job.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JJ.SmartHome.Job.dll"]