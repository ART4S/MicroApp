using Catalog.Application.Abstractions;
using Catalog.Application.Services.DataAccess;
using IntegrationServices.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Polly;
using System.Data.Common;

namespace Catalog.Application.PipelineBehaviours;

public class SaveChangesBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : Command<TResponse>
{
    private readonly ILogger _logger;
    private readonly ICatalogDbContext _catalogDb;
    private readonly IIntegrationDbContext _integrationDb;

    public SaveChangesBehaviour(
        ILogger<SaveChangesBehaviour<TRequest, TResponse>> logger,
        ICatalogDbContext catalogDb,
        IIntegrationDbContext integrationDb)
    {
        _logger = logger;
        _catalogDb = catalogDb;
        _integrationDb = integrationDb;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        using var transaction = await _catalogDb.Database.BeginTransactionAsync();

        _integrationDb.Database.UseTransaction(transaction.GetDbTransaction());

        TResponse response = await next();

        try
        {
            await Policy.Handle<DbException>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    (attempt) => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    (exception, _, attempt, _) => 
                    { 
                        // TODO: log
                    })
                .ExecuteAsync(() => transaction.CommitAsync(cancellationToken));
        }
        catch(Exception ex)
        {
            // TODO: log

            await transaction.RollbackAsync();
            throw;
        }

        return response;
    }
}
