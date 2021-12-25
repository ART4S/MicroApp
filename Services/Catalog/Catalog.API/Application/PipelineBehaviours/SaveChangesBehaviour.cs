using Catalog.API.Application.Requests.Abstractions;
using Catalog.API.DataAccess;
using IntegrationServices.EF;
using MediatR;
using Polly;
using System.Data.Common;

namespace Catalog.API.Application.PipelineBehaviours;

public class SaveChangesBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : Command<TResponse>
{
    private readonly ILogger _logger;
    private readonly ICatalogDbContext _catalogDb;
    private readonly IEFIntegrationDbContext _integrationDb;

    public SaveChangesBehaviour(
        ILogger<SaveChangesBehaviour<TRequest, TResponse>> logger,
        ICatalogDbContext catalogDb,
        IEFIntegrationDbContext integrationDb)
    {
        _logger = logger;
        _catalogDb = catalogDb;
        _integrationDb = integrationDb;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        TResponse response = await next();

        var policy = Policy.Handle<DbException>().WaitAndRetryAsync(
            retryCount: 3,
            (attempt) => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
            (exception, _, attempt, _) =>
            {
                // TODO: log
            });

        try
        {
            // TODO: Not safe
            await policy.ExecuteAsync(() => _catalogDb.SaveChangesAsync());
            await policy.ExecuteAsync(() => _integrationDb.SaveChangesAsync());
        }
        catch (Exception ex)
        {
            // TODO: log
            throw;
        }

        return response;
    }
}