using Catalog.API.Application.Requests.Abstractions;
using Catalog.API.DataAccess;
using IntegrationServices.Mongo;
using MediatR;
using Polly;
using System.Data.Common;

namespace Catalog.API.Application.PipelineBehaviours;

public class SaveChangesBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : Command<TResponse>
{
    private readonly ILogger _logger;
    private readonly ICatalogDbContext _catalogDb;
    private readonly IMongoIntegrationDbContext _integrationDb;

    public SaveChangesBehaviour(
        ILogger<SaveChangesBehaviour<TRequest, TResponse>> logger,
        ICatalogDbContext catalogDb,
        IMongoIntegrationDbContext integrationDb)
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
                _logger.LogError(
                    exception, 
                    "Error occured while saving changes for command {@Command} on {Attempt}", 
                    request, attempt);
            });

        try
        {
            // TODO: Use transaction. Current implementaion is not safe
            await policy.ExecuteAsync(() => _catalogDb.SaveChanges());
            await policy.ExecuteAsync(() => _integrationDb.SaveChanges());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Saving request {@Command} failed", request);
            throw;
        }

        return response;
    }
}