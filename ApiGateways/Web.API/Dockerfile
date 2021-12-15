FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
ARG BUILD_MODE=Debug
ENV BUILD_MODE=$BUILD_MODE
EXPOSE 80

WORKDIR /src
COPY "ApiGateways/Web.API/Web.API.csproj" "ApiGateways/Web.API/Web.API.csproj"
RUN dotnet restore "ApiGateways/Web.API/Web.API.csproj"
COPY . .
WORKDIR /src/ApiGateways/Web.API
RUN dotnet build --no-restore -c $BUILD_MODE
ENTRYPOINT dotnet run --no-build --no-launch-profile -c $BUILD_MODE