using Customer.Application.Abstractions.Messaging;
using Customer.Application.Email;
using Customer.Core.src.CustomerAggregate.Events;
using Customer.Core.src.Events;
using Customer.Core.src.Interfaces;

namespace Customer.Application.Events.Handlers;

public class CustomerCreatedHandler : IDomainEventHandler<CustomerCreatedEvent>
{
    private readonly IEmailSender _sender;

    public CustomerCreatedHandler(
        IEmailSender sender
    )
    {
        _sender = sender;
    }
    public async Task Handle(CustomerCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        var welcomeEmail = EmailTemplates.WelcomeEmail;
        var body = welcomeEmail.Body.Replace("{{CustomerName}}", domainEvent.Customer.Name.FirstName);
        var subject = welcomeEmail.Subject.Replace("{{CustomerName}}", domainEvent.Customer.Name.ToString());

        await _sender.SendEmailAsync(domainEvent.Customer.Email.Value, "no-replay@ourteamsupport.com", subject, body);
    }
}