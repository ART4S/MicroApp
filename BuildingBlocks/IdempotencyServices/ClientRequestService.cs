using IdempotencyServices.EF;
using Microsoft.EntityFrameworkCore;

namespace IdempotencyServices;

public class ClientRequestService : IClientRequestService
{
    private readonly IIdempotencyDbContext _idempotencyDb;
    private readonly Func<DateTime> _currentTime;

    public ClientRequestService(IIdempotencyDbContext idempotencyDb, Func<DateTime> currentTime)
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
