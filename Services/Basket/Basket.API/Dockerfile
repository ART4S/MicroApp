FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
ARG BUILD_MODE=Debug
ENV BUILD_MODE=$BUILD_MODE
EXPOSE 80

WORKDIR /src
COPY "Shared/IntegrationServices/IntegrationServices.csproj" "Shared/IntegrationServices/IntegrationServices.csproj"
COPY "EventBus/EventBus.Abstractions/EventBus.Abstractions.csproj" "EventBus/EventBus.Abstractions/EventBus.Abstractions.csproj"
COPY "EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj" "EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj"
COPY "EventBus/EventBus.RabbitMQ.DependencyInjection/EventBus.RabbitMQ.DependencyInjection.csproj" "EventBus/EventBus.RabbitMQ.DependencyInjection/EventBus.RabbitMQ.DependencyInjection.csproj"
COPY "Services/Basket/Basket.API/Basket.API.csproj" "Services/Basket/Basket.API/Basket.API.csproj"
RUN dotnet restore "Services/Basket/Basket.API/Basket.API.csproj"
COPY . .
WORKDIR /src/Services/Basket/Basket.API
RUN dotnet build --no-restore -c $BUILD_MODE
ENTRYPOINT dotnet run --no-build --no-launch-profile -c $BUILD_MODE