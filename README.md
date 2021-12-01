Creating migration for CatalogDbContext:
1. Run mssql container:  docker run -e ACCEPT_EULA=Y -e SA_PASSWORD=Qwerty123 -p 1433:1433 --name msssql -d mcr.microsoft.com/mssql/server
2. Execute command: dotnet ef migrations add Catalog_Initial -s ./Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj -p ./Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj -c CatalogDbContext -o DataAccess/Catalog/Migrations

Creating migrations for IntegrationDbContext:
1. Run mssql container:  docker run -e ACCEPT_EULA=Y -e SA_PASSWORD=Qwerty123 -p 1433:1433 --name msssql -d mcr.microsoft.com/mssql/server
Execute command: dotnet ef migrations add Integration_Initial -s ./Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj -p ./Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj -c IntegrationDbContext -o DataAccess/Integration/Migrations