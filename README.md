## Catalog
**CatalogDbContext**: dotnet ef migrations add Catalog_Initial -s ./Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj -p ./Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj -c CatalogDbContext -o DataAccess/Catalog/Migrations

**IntegrationDbContext**: dotnet ef migrations add Integration_Initial -s ./Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj -p ./Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj -c IntegrationDbContext -o DataAccess/Integration/Migrations

## Ordering
**OrderingDbContext**: dotnet ef migrations add Ordering_Initial -s ./Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj -p ./Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj -c OrderingDbContext -o DataAccess/Ordering/Migrations

**IntegrationDbContext**: dotnet ef migrations add Integration_Initial -s ./Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj -p ./Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj -c IntegrationDbContext -o DataAccess/Integration/Migrations

**IdempotencyDbContext** dotnet ef migrations add Idempotency_Initial -s ./Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj -p ./Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj -c IdempotencyDbContext -o DataAccess/Idempotency/Migrations

## Payment
**IntegrationDbContext**: dotnet ef migrations add Integration_Initial -s ./Services/Payment/Payment.API/Payment.API.csproj -p ./Services/Payment/Payment.API/Payment.API.csproj -c IntegrationDbContext -o DataAccess/Integration/Migrations


## Identity
**PersistedGrantDbContext** dotnet ef migrations add PersistedGrant_Initial -s ./Services/Identity/Identity.API/Identity.API.csproj -p ./Services/Identity/Identity.API/Identity.API.csproj -c PersistedGrantDbContext -o DataAccess/Migrations/PersistedGrant
**ConfigurationDbContext** dotnet ef migrations add Configuration_Initial  -s ./Services/Identity/Identity.API/Identity.API.csproj -p ./Services/Identity/Identity.API/Identity.API.csproj -c ConfigurationDbContext -o DataAccess/Migrations/Configuration
**AppDbContext** dotnet ef migrations add App_Initial -s ./Services/Identity/Identity.API/Identity.API.csproj -p ./Services/Identity/Identity.API/Identity.API.csproj -c AppDbContext -o DataAccess/Migrations/App

## GRPCUI
grpcui -plaintext localhost:5001