namespace EventBus.RabbitMQ;

public record RabbitMQSettings(
    string ClientName, string HostName, 
    string UserName, string Password, int Retries);
