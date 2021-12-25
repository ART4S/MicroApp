using StackExchange.Redis;

namespace Basket.API.Configuration;

static class HostConfiguration
{
    public static IHost InitDatabase(this IHost host)
    {
        var config = host.Services.GetRequiredService<IConfiguration>();

        bool createDb = config.GetValue<bool>("ClearDatabase");

        if (createDb)
        {
            var connection = host.Services.GetRequiredService<IConnectionMultiplexer>();
            var endpoint = connection.GetEndPoints().FirstOrDefault();
            if (endpoint != null)
            {
                var server = connection.GetServer(endpoint);
                if (server != null)
                    server.FlushAllDatabases();
            }
        }

        return host;
    }
}
