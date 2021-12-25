namespace EventBus.RabbitMQ;

public class RabbitMQSettings
{
    public string Uri { get; set; }
    public string ClientName { get; set; }
    public int Retries { get; set; }
}
