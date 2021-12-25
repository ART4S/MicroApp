FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
ARG BUILD_MODE=Debug
ENV BUILD_MODE=$BUILD_MODE
EXPOSE 80

WORKDIR /src
COPY "Clients/WebHealthMonitor/WebHealthMonitor.csproj" "Clients/WebHealthMonitor/WebHealthMonitor.csproj"
RUN dotnet restore "Clients/WebHealthMonitor/WebHealthMonitor.csproj"
COPY . .
WORKDIR /src/Clients/WebHealthMonitor
RUN dotnet build --no-restore -c $BUILD_MODE
ENTRYPOINT dotnet run --no-build --no-launch-profile -c $BUILD_MODE