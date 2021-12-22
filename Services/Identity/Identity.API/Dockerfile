FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
ARG BUILD_MODE=Debug
ENV BUILD_MODE=$BUILD_MODE
EXPOSE 80

WORKDIR /src
COPY "BuildingBlocks/HostConfiguration/HostConfiguration.csproj" "BuildingBlocks/HostConfiguration/HostConfiguration.csproj"
COPY "Services/Identity/Identity.API/Identity.API.csproj" "Services/Identity/Identity.API/Identity.API.csproj"
RUN dotnet restore "Services/Identity/Identity.API/Identity.API.csproj"
COPY . .
WORKDIR /src/Services/Identity/Identity.API
RUN dotnet build --no-restore -c $BUILD_MODE
ENTRYPOINT dotnet run --no-build --no-launch-profile -c $BUILD_MODE