#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Daemons/WindParkAPIDaemon/WindParkAPI.Daemon/WindParkAPI.Daemon.csproj", "Services/Daemons/WindParkAPIDaemon/WindParkAPI.Daemon/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
RUN dotnet restore "Services/Daemons/WindParkAPIDaemon/WindParkAPI.Daemon/WindParkAPI.Daemon.csproj"
COPY . .
WORKDIR "/src/Services/Daemons/WindParkAPIDaemon/WindParkAPI.Daemon"
RUN dotnet build "WindParkAPI.Daemon.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WindParkAPI.Daemon.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WindParkAPI.Daemon.dll"]