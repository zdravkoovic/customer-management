namespace Customer.Core.src.Events;

public class PaymentReceivedEvent : IIntegrationEvent
{
    public int Version { get; init; }

    public string EventType => nameof(PaymentReceivedEvent);

    public Guid Id { get; init; }

    public DateTimeOffset OccurredOn { get; init; }

    public Guid AggregateId { get; init; }

    public required string OrderReference { get; init; }
    public decimal Amount { get; init; }
    public required string PaymentMethod { get; init; }
    public required string CustomerFirstname { get; init; }
    public required string CustomerLastname { get; init; }
    public required string CustomerEmail { get; init; }

    public string ToJson()
    {
        throw new NotImplementedException();
    }
}