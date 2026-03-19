using Customer.Application.Abstractions;
using Customer.Application.Abstractions.Mappers;
using Customer.Application.Abstractions.Messaging;
using Customer.Application.Configuration;


// using Customer.Application.Behaviours;
using Customer.Application.Customer.IntegrationEvents;
using Customer.Application.Customer.IntegrationEvents.Handlers;
using Customer.Application.Customer.Queries.GetCustomer;
using Customer.Application.Customer.Repositories;
using Customer.Application.Events.Dispatchers;
using Customer.Application.Events.Handlers;
using Customer.Core.src.CustomerAggregate.Events;
using Customer.Core.src.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Customer.Application.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatchers>();
        services.AddScoped<IIntegrationEventResolver, IntegrationEventResolver>();
        services.AddScoped<IDomainEventHandler<CustomerCreatedEvent>, CustomerCreatedHandler>();

        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssemblies(typeof(GetCustomerQuery).Assembly);
            // conf.AddOpenBehavior(typeof(ExceptionHandlingPipelineBehavior<,>));
            // conf.AddOpenBehavior(typeof(LoggingPipelineBehaviour<,>));
            // conf.AddOpenBehavior(typeof(ValidationPipelineBehaviours<,>));
        });

        services.Scan(scan => scan
            .FromAssemblyOf<IDomainEventHandler<IDomainEvent>>()
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        services.Scan(scan => scan
            .FromAssemblyOf<IIntegrationEventMapper>()
            .AddClasses(c => c.AssignableTo(typeof(IIntegrationEventMapper<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );  

        services.AddAutoMapper(cfg => {}, typeof(ApplicationServiceCollectionExtensions).Assembly);

        services.AddScoped<
            IIntegrationEventHandler<PaymentReceivedEvent>,
            PaymentReceivedEventHandler>();
        

        return services;
    }
}