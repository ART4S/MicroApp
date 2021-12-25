using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using Web.API.Models.Dictionaries;
using Web.API.Models.Orders;
using Web.API.Models.PaymentMethods;
using Web.API.Settings;
using Web.API.Utils;

namespace Web.API.Services;

public class OrderingService : IOrderingService
{
    private readonly ILogger _logger;
    private readonly HttpClient _orderingClient;
    private readonly OrderingUrls _urls;

    public OrderingService(
        ILogger<OrderingService> logger,
        HttpClient orderingClient, OrderingUrls urls)
    {
        _logger = logger;
        _orderingClient = orderingClient;
        _urls = urls;
    }

    public async Task<ICollection<OrderSummaryDto>> GetOrders()
    {
        HttpResponseMessage response = await _orderingClient.GetAsync(_urls.GetOrdersUrl);

        await HttpUtils.HandleErrorStatusCodes(response);

        return await response.Content.ReadFromJsonAsync<OrderSummaryDto[]>();
    }

    public async Task<OrderInfoDto> GetById(Guid orderId)
    {
        string uri = Regex.Replace(_urls.GetByIdUrl, "{id}", orderId.ToString());

        HttpResponseMessage response = await _orderingClient.GetAsync(uri);

        await HttpUtils.HandleErrorStatusCodes(response);

        return await response.Content.ReadFromJsonAsync<OrderInfoDto>();
    }

    public async Task ConfirmOrder(Guid requestId, Guid orderId, OrderEditDto order)
    {
        string uri = Regex.Replace(_urls.ConfirmOrderUrl, "{id}", orderId.ToString());

        using HttpRequestMessage request = new(HttpMethod.Put, uri);

        request.Headers.Add("x-requestId", requestId.ToString());

        request.Content = JsonContent.Create(order);

        HttpResponseMessage response = await _orderingClient.SendAsync(request);

        await HttpUtils.HandleErrorStatusCodes(response);
    }

    public async Task<ICollection<PaymentMethodInfoDto>> GetPaymentMethods()
    {
        HttpResponseMessage response = await _orderingClient.GetAsync(_urls.GetPaymentMethodsUrl);

        await HttpUtils.HandleErrorStatusCodes(response);

        return await response.Content.ReadFromJsonAsync<PaymentMethodInfoDto[]>();
    }

    public async Task<Guid> CreatePaymentMethod(PaymentMethodEditDto paymentMethod)
    {
        HttpResponseMessage response = await _orderingClient.PostAsJsonAsync(_urls.CreatePaymentMethodUrl, paymentMethod);

        await HttpUtils.HandleErrorStatusCodes(response);

        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    public async Task<ICollection<OrderStatusDictDto>> GetOrderStatusesDict()
    {
        HttpResponseMessage response = await _orderingClient.GetAsync(_urls.GetOrderStatusesDictUrl);

        await HttpUtils.HandleErrorStatusCodes(response);

        return await response.Content.ReadFromJsonAsync<OrderStatusDictDto[]>();
    }

    public async Task<ICollection<CardTypeDictDto>> GetCardTypesDict()
    {
        HttpResponseMessage response = await _orderingClient.GetAsync(_urls.GetCardTypesDictUrl);

        await HttpUtils.HandleErrorStatusCodes(response);

        return await response.Content.ReadFromJsonAsync<CardTypeDictDto[]>();
    }
}