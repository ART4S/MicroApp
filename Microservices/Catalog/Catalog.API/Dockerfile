FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
ARG BUILD_MODE=Debug
ENV BUILD_MODE=$BUILD_MODE
EXPOSE 80

WORKDIR /src
COPY "Common/WebHost.Customization/WebHost.Customization.csproj" "Common/WebHost.Customization/WebHost.Customization.csproj"
COPY "Microservices/Catalog/Catalog.Domian/Catalog.Domian.csproj" "Microservices/Catalog/Catalog.Domian/Catalog.Domian.csproj"
COPY "Microservices/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj" "Microservices/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj"
COPY "Microservices/Catalog/Catalog.API/Catalog.API.csproj" "Microservices/Catalog/Catalog.API/Catalog.API.csproj"
RUN dotnet restore "Microservices/Catalog/Catalog.API/Catalog.API.csproj"
COPY . .
WORKDIR /src/Microservices/Catalog/Catalog.API
RUN dotnet build --no-restore -c $BUILD_MODE
ENTRYPOINT dotnet run --no-build --no-launch-profile -c $BUILD_MODE