using Web.API.Models.Dictionaries;
using Web.API.Models.Orders;
using Web.API.Models.PaymentMethods;

namespace Web.API.Services.Ordering;

public interface IOrderingService
{
    Task<ICollection<OrderSummaryDto>> GetOrders();
    Task<OrderInfoDto> GetById(Guid orderId);
    Task ConfirmOrder(Guid requestId, Guid orderId, OrderEditDto order);
    Task<ICollection<PaymentMethodInfoDto>> GetPaymentMethods();
    Task<Guid> CreatePaymentMethod(PaymentMethodEditDto paymentMethod);
    Task<ICollection<OrderStatusDictDto>> GetOrderStatusesDict();
    Task<ICollection<CardTypeDictDto>> GetCardTypesDict();
}
