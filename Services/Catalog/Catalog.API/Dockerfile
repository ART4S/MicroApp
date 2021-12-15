FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
ARG BUILD_MODE=Debug
ENV BUILD_MODE=$BUILD_MODE
EXPOSE 80

WORKDIR /src
COPY "BuildingBlocks/HostConfiguration/HostConfiguration.csproj" "BuildingBlocks/HostConfiguration/HostConfiguration.csproj"
COPY "BuildingBlocks/IntegrationServices/IntegrationServices.csproj" "BuildingBlocks/IntegrationServices/IntegrationServices.csproj"
COPY "BuildingBlocks/TaskScheduling/TaskScheduling.csproj" "BuildingBlocks/TaskScheduling/TaskScheduling.csproj"
COPY "BuildingBlocks/EventBus/EventBus.Abstractions/EventBus.Abstractions.csproj" "BuildingBlocks/EventBus/EventBus.Abstractions/EventBus.Abstractions.csproj"
COPY "BuildingBlocks/EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj" "BuildingBlocks/EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj"
COPY "BuildingBlocks/EventBus/EventBus.RabbitMQ.DependencyInjection/EventBus.RabbitMQ.DependencyInjection.csproj" "BuildingBlocks/EventBus/EventBus.RabbitMQ.DependencyInjection/EventBus.RabbitMQ.DependencyInjection.csproj"
COPY "Services/Catalog/Catalog.Domian/Catalog.Domian.csproj" "Services/Catalog/Catalog.Domian/Catalog.Domian.csproj"
COPY "Services/Catalog/Catalog.Application/Catalog.Application.csproj" "Services/Catalog/Catalog.Application/Catalog.Application.csproj"
COPY "Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj" "Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj"
COPY "Services/Catalog/Catalog.API/Catalog.API.csproj" "Services/Catalog/Catalog.API/Catalog.API.csproj"
RUN dotnet restore "Services/Catalog/Catalog.API/Catalog.API.csproj"
COPY . .
WORKDIR /src/Services/Catalog/Catalog.API
RUN dotnet build --no-restore -c $BUILD_MODE
ENTRYPOINT dotnet run --no-build --no-launch-profile -c $BUILD_MODE