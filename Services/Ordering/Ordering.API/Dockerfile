FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
ARG BUILD_MODE=Debug
ENV BUILD_MODE=$BUILD_MODE
EXPOSE 80

WORKDIR /src
COPY "Shared/HostConfiguration/HostConfiguration.csproj" "Shared/HostConfiguration/HostConfiguration.csproj"
COPY "Shared/IntegrationServices/IntegrationServices.csproj" "Shared/IntegrationServices/IntegrationServices.csproj"
COPY "Shared/IdempotencyServices/IdempotencyServices.csproj" "Shared/IdempotencyServices/IdempotencyServices.csproj"
COPY "Shared/TaskScheduling/TaskScheduling.csproj" "Shared/TaskScheduling/TaskScheduling.csproj"
COPY "EventBus/EventBus.Abstractions/EventBus.Abstractions.csproj" "EventBus/EventBus.Abstractions/EventBus.Abstractions.csproj"
COPY "EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj" "EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj"
COPY "EventBus/EventBus.RabbitMQ.DependencyInjection/EventBus.RabbitMQ.DependencyInjection.csproj" "EventBus/EventBus.RabbitMQ.DependencyInjection/EventBus.RabbitMQ.DependencyInjection.csproj"
COPY "Services/Ordering/Ordering.Domian/Ordering.Domian.csproj" "Services/Ordering/Ordering.Domian/Ordering.Domian.csproj"
COPY "Services/Ordering/Ordering.Application/Ordering.Application.csproj" "Services/Ordering/Ordering.Application/Ordering.Application.csproj"
COPY "Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj" "Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj"
COPY "Services/Ordering/Ordering.API/Ordering.API.csproj" "Services/Ordering/Ordering.API/Ordering.API.csproj"
RUN dotnet restore "Services/Ordering/Ordering.API/Ordering.API.csproj"
COPY . .
WORKDIR /src/Services/Ordering/Ordering.API
RUN dotnet build --no-restore -c $BUILD_MODE
ENTRYPOINT dotnet run --no-build --no-launch-profile -c $BUILD_MODE