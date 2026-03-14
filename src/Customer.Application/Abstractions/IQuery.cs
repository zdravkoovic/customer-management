using CSharpFunctionalExtensions;
using Customer.Core.src.Errors;
using MediatR;

namespace Customer.Application.Abstractions;

public interface IRequestBase {}

public interface IQuery<TResponse> : IRequestBase, IRequest<Result<TResponse, IDomainError>>
    where TResponse : notnull
{}