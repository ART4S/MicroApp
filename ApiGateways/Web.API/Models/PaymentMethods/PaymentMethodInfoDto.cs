namespace Web.API.Models.PaymentMethods;

public class PaymentMethodInfoDto
{
    public Guid Id { get; set; }

    public string Alias { get; set; }

    public int CardTypeId { get; set; }
}
