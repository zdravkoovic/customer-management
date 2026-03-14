using FluentValidation;

namespace Customer.Application.Customer.Commands.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.FirstName).NotNull().MaximumLength(255);
        RuleFor(x => x.LastName).NotNull().MaximumLength(255);
        RuleFor(x => x.Email).NotNull().MaximumLength(255).EmailAddress();
        RuleFor(x => x.HouseNumber).MaximumLength(255);
        RuleFor(x => x.ZipCode).MaximumLength(255);
        RuleFor(x => x.Street).Null().MaximumLength(255);
    }
}