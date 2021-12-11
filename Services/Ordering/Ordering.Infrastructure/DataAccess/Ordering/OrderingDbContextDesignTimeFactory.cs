using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ordering.Infrastructure.DataAccess.Ordering;

internal class OrderingDbContextDesignTimeFactory : IDesignTimeDbContextFactory<OrderingDbContext>
{
    public OrderingDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<OrderingDbContext> builder = new();

        builder.UseSqlServer("Server=.;Port=1433;Database=ordering_db;User Id=sa;Password=Qwerty123", options =>
        {
            options.MigrationsAssembly(GetType().Assembly.FullName);
        });

        return new OrderingDbContext(builder.Options);
    }
}
