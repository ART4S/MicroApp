namespace Web.API.Settings;

public record OrderingUrls
{
    public string BasePath { get; set; }
    public string GetOrdersUrl { get; set; }
    public string GetByIdUrl { get; set; }
    public string ConfirmOrderUrl { get; set; }
    public string GetPaymentMethodsUrl { get; set; }
    public string CreatePaymentMethodUrl { get; set; }
    public string GetOrderStatusesDictUrl { get; set; }
    public string GetCardTypesDictUrl { get; set; }
}
