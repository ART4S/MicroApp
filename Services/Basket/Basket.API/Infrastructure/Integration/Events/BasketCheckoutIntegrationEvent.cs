using Basket.API.Model;
using IntegrationServices;

namespace Basket.API.Infrastructure.Integration.Events;

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

    public Guid RequestId { get; private init; }

    public string City { get; private init; }

    public string Street { get; private init; }

    public string State { get; private init; }

    public string Country { get; private init; }

    public string ZipCode { get; private init; }

    public string CardNumber { get; private init; }

    public string CardHolderName { get; private init; }

    public DateTime CardExpiration { get; private init; }

    public string CardSecurityNumber { get; private init; }

    public Guid CardTypeId { get; private init; }

    public CustomerBasket Basket { get; private init; }
}
