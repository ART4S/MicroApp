namespace IdempotencyServices;

public interface IClientRequestService
{
    Task<bool> Exists(Guid requestId);
    Task Create(Guid requestId, string name);
}
