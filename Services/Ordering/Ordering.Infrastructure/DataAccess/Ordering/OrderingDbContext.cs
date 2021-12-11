using Microsoft.EntityFrameworkCore;
using Ordering.Application.Services.DataAccess;
using Ordering.Domian.Aggregates.OrderAggregate;
using Ordering.Domian.Dictionaries;
using Ordering.Domian.Entities.BuyerAggregate;
using Ordering.Domian.Entities.OrderAggregate;

namespace Ordering.Infrastructure.DataAccess.Ordering;

public class OrderingDbContext : DbContext, IOrderingDbContext
{
    public OrderingDbContext(DbContextOptions<OrderingDbContext> options): base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Buyer> Buyers { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<CardTypeDict> CardTypesDict { get; set; }
    public DbSet<OrderStatusDict> OrderStatusesDict { get; set; }
}
