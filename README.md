# Customer Management Service

A .NET 10 backend service for managing customer profiles.

The service follows Hexagonal / Clean Architecture and is split into four layers:

- `Customer.Core`
- `Customer.Application`
- `Customer.Infrastructure`
- `Customer.API`

The runnable host is `src/Customer.API`.

## Features

- Create and manage customer profiles
- REST API for customer operations
- PostgreSQL persistence with EF Core
- Keycloak-based authentication/authorization
- Kafka background publishing/consuming
- Outbox pattern for reliable integration event storage
- Integration event resolution from domain events

## Tech Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Kafka
- SMTP
- Keycloak

## Project Structure

- `src/Customer.API`  
  API entry point. Contains `Program.cs`, controllers, middleware, and request pipeline setup.

- `src/Customer.Application`  
  Application layer. Contains commands, queries, handlers, contracts, and orchestration logic.

- `src/Customer.Infrastructure`  
  Infrastructure layer. Contains EF Core, persistence, Kafka integration, SMTP integration, and other external implementations.

- `src/Customer.Core`  
  Domain layer. Contains entities, aggregates, domain events, and business rules.

## Architecture Notes

This service uses a Clean Architecture approach.

The application layer is built around commands and queries. Command handlers inherit from a shared `CommandHandlerBase` class. The handler flow is:

1. execute `ExecuteAsync`
2. execute `SaveIntegrationEventsAsync`
3. perform Unit of Work / transaction save
4. dispatch domain events if the result is an `IAggregateRoot`
5. return the result

The service also uses an Outbox pattern so integration events are stored in the same database transaction as the business change.

An `IntegrationEventResolver` maps domain events to integration events dynamically. This keeps command handlers smaller and makes event publishing easier.

Example flow:

```csharp
var domainEvents = _createdCustomer?.DomainEvents.ToList();
if(domainEvents == null || domainEvents.Count == 0) return;

var integrationEvents = _integrationEventResolver.Resolve(domainEvents);
await _integrationEventLog.AddIntegrationEventsAsync([.. integrationEvents], cancellationToken);