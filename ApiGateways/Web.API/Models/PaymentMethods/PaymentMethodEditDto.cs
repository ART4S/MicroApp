namespace Web.API.Models.PaymentMethods;

public class PaymentMethodEditDto
{
    public string Alias { get; set; }

    public string CardNumber { get; set; }

    public string SecurityNumber { get; set; }

    public string CardHolderName { get; set; }

    public DateTime Expiration { get; set; }

    public int CardTypeId { get; set; }
}
