FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
ARG BUILD_MODE=Debug
ENV BUILD_MODE=$BUILD_MODE
EXPOSE 80

WORKDIR /src
COPY "BuildingBlocks/HostConfiguration/HostConfiguration.csproj" "BuildingBlocks/HostConfiguration/HostConfiguration.csproj"
COPY "BuildingBlocks/IntegrationServices/IntegrationServices.csproj" "BuildingBlocks/IntegrationServices/IntegrationServices.csproj"
COPY "BuildingBlocks/IdempotencyServices/IdempotencyServices.csproj" "BuildingBlocks/IdempotencyServices/IdempotencyServices.csproj"
COPY "BuildingBlocks/TaskScheduling/TaskScheduling.csproj" "BuildingBlocks/TaskScheduling/TaskScheduling.csproj"
COPY "BuildingBlocks/EventBus/EventBus.Abstractions/EventBus.Abstractions.csproj" "BuildingBlocks/EventBus/EventBus.Abstractions/EventBus.Abstractions.csproj"
COPY "BuildingBlocks/EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj" "BuildingBlocks/EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj"
COPY "BuildingBlocks/EventBus/EventBus.RabbitMQ.DependencyInjection/EventBus.RabbitMQ.DependencyInjection.csproj" "BuildingBlocks/EventBus/EventBus.RabbitMQ.DependencyInjection/EventBus.RabbitMQ.DependencyInjection.csproj"
COPY "Services/Ordering/Ordering.Domian/Ordering.Domian.csproj" "Services/Ordering/Ordering.Domian/Ordering.Domian.csproj"
COPY "Services/Ordering/Ordering.Application/Ordering.Application.csproj" "Services/Ordering/Ordering.Application/Ordering.Application.csproj"
COPY "Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj" "Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj"
COPY "Services/Ordering/Ordering.API/Ordering.API.csproj" "Services/Ordering/Ordering.API/Ordering.API.csproj"
RUN dotnet restore "Services/Ordering/Ordering.API/Ordering.API.csproj"
COPY . .
WORKDIR /src/Services/Ordering/Ordering.API
RUN dotnet build --no-restore -c $BUILD_MODE
ENTRYPOINT dotnet run --no-build --no-launch-profile -c $BUILD_MODE