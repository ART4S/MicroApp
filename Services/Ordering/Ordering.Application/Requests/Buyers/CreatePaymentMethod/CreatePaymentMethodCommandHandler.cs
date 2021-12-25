using AutoMapper;
using MediatR;
using Ordering.Application.Services;
using Ordering.Domian.Entities;

namespace Ordering.Application.Requests.Buyers.CreatePaymentMethod;

public class CreatePaymentMethodCommandHandler : IRequestHandler<CreatePaymentMethodCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IOrderingDbContext _orderingDb;

    public CreatePaymentMethodCommandHandler(IMapper mapper, IOrderingDbContext orderingDb)
    {
        _mapper = mapper;
        _orderingDb = orderingDb;
    }

    public async Task<Guid> Handle(CreatePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        PaymentMethod method = new() { BuyerId = request.BuyerId };

        _mapper.Map(request.PaymentMethod, method);

        await _orderingDb.PaymentMethods.AddAsync(method);

        await _orderingDb.SaveChangesAsync();

        return method.Id;
    }
}