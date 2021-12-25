FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
ARG BUILD_MODE=Debug
ENV BUILD_MODE=$BUILD_MODE
EXPOSE 80

WORKDIR /src
COPY "BuildingBlocks/IntegrationServices/IntegrationServices.csproj" "BuildingBlocks/IntegrationServices/IntegrationServices.csproj"
COPY "BuildingBlocks/EventBus/EventBus.Abstractions/EventBus.Abstractions.csproj" "BuildingBlocks/EventBus/EventBus.Abstractions/EventBus.Abstractions.csproj"
COPY "BuildingBlocks/EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj" "BuildingBlocks/EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj"
COPY "BuildingBlocks/EventBus/EventBus.RabbitMQ.DependencyInjection/EventBus.RabbitMQ.DependencyInjection.csproj" "BuildingBlocks/EventBus/EventBus.RabbitMQ.DependencyInjection/EventBus.RabbitMQ.DependencyInjection.csproj"
COPY "Services/Ordering/Ordering.SignalR/Ordering.SignalR.csproj" "Services/Ordering/Ordering.SignalR/Ordering.SignalR.csproj"
RUN dotnet restore "Services/Ordering/Ordering.SignalR/Ordering.SignalR.csproj"
COPY . .
WORKDIR /src/Services/Ordering/Ordering.SignalR
RUN dotnet build --no-restore -c $BUILD_MODE
ENTRYPOINT dotnet run --no-build --no-launch-profile -c $BUILD_MODE