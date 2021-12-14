using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Ordering.Domian.Dictionaries;
using Ordering.Domian.Entities;

namespace Ordering.Application.Services.DataAccess;

public interface IOrderingDbContext
{
    DatabaseFacade Database { get; }

    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<Buyer> Buyers { get; }
    DbSet<PaymentMethod> PaymentMethods { get; }
    DbSet<CardTypeDict> CardTypesDict { get; }
    DbSet<OrderStatusDict> OrderStatusesDict { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
