using Basket.API.Model;
using IntegrationServices;

namespace Basket.API.Infrastructure.IntegrationEvents;

public record BasketCheckoutIntegrationEvent : IntegrationEvent
{
    public BasketCheckoutIntegrationEvent(
        Guid requestId, string city, string street, string state, 
        string country, string zipCode, string cardNumber, string cardHolderName,
        DateTime cardExpiration, string cardSecurityNumber, Guid cardTypeId, CustomerBasket basket)
    {
        RequestId = requestId;
        City = city;
        Street = street;
        State = state;
        Country = country;
        ZipCode = zipCode;
        CardNumber = cardNumber;
        CardHolderName = cardHolderName;
        CardExpiration = cardExpiration;
        CardSecurityNumber = cardSecurityNumber;
        CardTypeId = cardTypeId;
        Basket = basket;
    }

    public Guid RequestId { get; private set; }

    public string City { get; private set; }

    public string Street { get; private set; }

    public string State { get; private set; }

    public string Country { get; private set; }

    public string ZipCode { get; private set; }

    public string CardNumber { get; private set; }

    public string CardHolderName { get; private set; }

    public DateTime CardExpiration { get; private set; }

    public string CardSecurityNumber { get; private set; }

    public Guid CardTypeId { get; private set; }

    public CustomerBasket Basket { get; private set; }
}
