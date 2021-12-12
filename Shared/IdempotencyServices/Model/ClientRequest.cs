namespace IdempotencyServices.Model;

public class ClientRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTime CreationDate { get; set; }
}
