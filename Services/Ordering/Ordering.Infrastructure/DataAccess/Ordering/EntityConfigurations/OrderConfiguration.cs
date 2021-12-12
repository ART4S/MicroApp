using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domian.Aggregates.OrderAggregate;

namespace Ordering.Infrastructure.DataAccess.Ordering.EntityConfigurations;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(x => x.Address, x =>
        {
            x.ToTable("OrderAddresses");
        });
    }
}
