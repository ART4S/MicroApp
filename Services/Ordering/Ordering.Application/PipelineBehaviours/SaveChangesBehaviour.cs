using IntegrationServices.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Ordering.Application.Requests.Abstractions;
using Ordering.Application.Services;
using Polly;
using System.Data.Common;

namespace Ordering.Application.PipelineBehaviours;

public class SaveChangesBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : Command<TResponse>
{
    private readonly ILogger _logger;
    private readonly IOrderingDbContext _orderingDb;
    private readonly IEFIntegrationDbContext _integrationDb;

    public SaveChangesBehaviour(
        ILogger<SaveChangesBehaviour<TRequest, TResponse>> logger,
        IOrderingDbContext orderingDb,
        IEFIntegrationDbContext integrationDb)
    {
        _logger = logger;
        _orderingDb = orderingDb;
        _integrationDb = integrationDb;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        using var transaction = await _orderingDb.Database.BeginTransactionAsync();

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
                        _logger.LogError(exception, "Error occured while saving changes on attempt {Attempt}", attempt);
                    })
                .ExecuteAsync(() => transaction.CommitAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Saving changes failed. Rolling back transaction...");

            await transaction.RollbackAsync();

            throw;
        }

        return response;
    }
}
