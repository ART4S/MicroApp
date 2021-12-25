using Microsoft.EntityFrameworkCore;

namespace IdempotencyServices.EF;

public class EFClientRequestService : IClientRequestService
{
    private readonly EFIdempotencyDbContext _idempotencyDb;
    private readonly Func<DateTime> _currentTime;

    public EFClientRequestService(EFIdempotencyDbContext idempotencyDb, Func<DateTime> currentTime)
    {
        _idempotencyDb = idempotencyDb;
        _currentTime = currentTime;
    }

    public async Task Create(Guid requestId, string name)
    {
        await _idempotencyDb.ClientRequests.AddAsync(new()
        {
            Id = requestId,
            Name = name,
            CreationDate = _currentTime()
        });

        await _idempotencyDb.SaveChangesAsync();
    }

    public Task<bool> Exists(Guid requestId)
    {
        return _idempotencyDb.ClientRequests.AnyAsync(x => x.Id == requestId);
    }
}
