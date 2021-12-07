using Basket.API.Model;
using IntegrationServices;

namespace Basket.API.Infrastructure.IntegrationEvents;

public record BasketCheckoutIntegrationEvent : IntegrationEvent
{
    public string City { get; set; }

    public string Street { get; private set; }

    public string State { get; private set; }

    public string Country { get; private set; }

    public string ZipCode { get; set; }

    public string CardNumber { get; set; }

    public string CardHolderName { get; set; }

    public DateTime CardExpiration { get; set; }

    public string CardSecurityNumber { get; set; }

    public int CardTypeId { get; set; }

    public string Buyer { get; set; }

    public Guid RequestId { get; set; }

    public CustomerBasket Basket { get; set; }
}
