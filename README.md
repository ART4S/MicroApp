## MSSQL Container
docker run -e ACCEPT_EULA=Y -e SA_PASSWORD=Qwerty123 -p 1433:1433 --name msssql -d mcr.microsoft.com/mssql/server

## Catalog
CatalogDbContext: dotnet ef migrations add Catalog_Initial -s ./Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj -p ./Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj -c CatalogDbContext -o DataAccess/Catalog/Migrations
IntegrationDbContext: dotnet ef migrations add Integration_Initial -s ./Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj -p ./Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj -c IntegrationDbContext -o DataAccess/Integration/Migrations

## Ordering
OrderingDbContext: dotnet ef migrations add Ordering_Initial -s ./Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj -p ./Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj -c OrderingDbContext -o DataAccess/Ordering/Migrations
IntegrationDbContext: dotnet ef migrations add Integration_Initial -s ./Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj -p ./Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj -c IntegrationDbContext -o DataAccess/Integration/Migrations