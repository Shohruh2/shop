using FluentValidation;
using Shop.Api.Contracts.Requests;

namespace Shop.Api.Validators;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.Items).NotEmpty();
    }
}

public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderRequestValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.DateTime).NotEmpty();
        RuleFor(x => x.OrderItems).NotEmpty();
        RuleFor(x => x.TotalPrice).NotEmpty();
    }
}