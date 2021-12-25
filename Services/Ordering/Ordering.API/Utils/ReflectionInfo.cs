using Ordering.Application.Services;
using Ordering.Infrastructure.DataAccess.Ordering;
using System.Reflection;

namespace Ordering.API.Utils;

public static class ReflectionInfo
{
    public static Assembly ApplicationAssembly
        => typeof(IOrderingDbContext).Assembly;

    public static Assembly InfrastructureAssembly
        => typeof(OrderingDbContext).Assembly;
}
