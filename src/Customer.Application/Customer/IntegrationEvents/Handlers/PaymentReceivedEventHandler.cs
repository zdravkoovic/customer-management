using Customer.Application.Customer.Repositories;
using Customer.Core.src.Events;
using Microsoft.Extensions.Logging;

namespace Customer.Application.Customer.IntegrationEvents.Handlers;

public class PaymentReceivedEventHandler : IIntegrationEventHandler<PaymentReceivedEvent>
{
    private readonly ILogger<PaymentReceivedEventHandler> _logger;
    private readonly ICustomerRepository _customerRepository;
    public PaymentReceivedEventHandler(ILogger<PaymentReceivedEventHandler> logger, ICustomerRepository customerRepository)
    {
        _logger = logger;
        _customerRepository = customerRepository;
    }

    public Task HandleAsync(PaymentReceivedEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Payment received for order {OrderReference}, amount: {Amount}",
            @event.OrderReference, 
            @event.Amount
        );
        return Task.CompletedTask;
    }
}