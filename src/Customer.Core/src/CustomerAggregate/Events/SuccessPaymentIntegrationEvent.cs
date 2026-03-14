using Customer.Core.src.Events;

namespace Customer.Core.src.CustomerAggregate.Events;

public class SuccessPaymentIntegrationEvent(Guid aggregateId, Guid customerId, decimal amount, DateTime paymentDate) : IntegrationEvent(aggregateId)
{
    public Guid CustomerId { get; } = customerId;
    public decimal Amount { get; } = amount;
    public DateTime PaymentDate { get; } = paymentDate;
    public int OrderId { get; init; }
}