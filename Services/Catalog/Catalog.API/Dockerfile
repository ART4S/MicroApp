FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
ARG BUILD_MODE=Debug
ENV BUILD_MODE=$BUILD_MODE
EXPOSE 80

WORKDIR /src
COPY "Shared/HostConfiguration/HostConfiguration.csproj" "Shared/HostConfiguration/HostConfiguration.csproj"
COPY "Shared/IntegrationServices/IntegrationServices.csproj" "Shared/IntegrationServices/IntegrationServices.csproj"
COPY "Services/Catalog/Catalog.Domian/Catalog.Domian.csproj" "Services/Catalog/Catalog.Domian/Catalog.Domian.csproj"
COPY "Services/Catalog/Catalog.Application/Catalog.Application.csproj" "Services/Catalog/Catalog.Application/Catalog.Application.csproj"
COPY "Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj" "Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj"
COPY "Services/Catalog/Catalog.API/Catalog.API.csproj" "Services/Catalog/Catalog.API/Catalog.API.csproj"
COPY "EventBus/EventBus.Abstractions/EventBus.Abstractions.csproj" "EventBus/EventBus.Abstractions/EventBus.Abstractions.csproj"
COPY "EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj" "EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj"
COPY "EventBus/EventBus.RabbitMQ.DependencyInjection/EventBus.RabbitMQ.DependencyInjection.csproj" "EventBus/EventBus.RabbitMQ.DependencyInjection/EventBus.RabbitMQ.DependencyInjection.csproj"
RUN dotnet restore "Services/Catalog/Catalog.API/Catalog.API.csproj"
COPY . .
WORKDIR /src/Services/Catalog/Catalog.API
RUN dotnet build --no-restore -c $BUILD_MODE
ENTRYPOINT dotnet run --no-build --no-launch-profile -c $BUILD_MODE