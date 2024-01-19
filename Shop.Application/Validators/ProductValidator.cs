using FluentValidation;
using Shop.Domain.Products;

namespace Shop.Application.Validators;

public class ProductValidator : AbstractValidator<Product>
{

    public ProductValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        RuleFor(x => x.Description)
            .NotEmpty();
        RuleFor(x => x.Name)
            .NotEmpty();
        RuleFor(x => x.Price)
            .NotEmpty();
        RuleFor(x => x.Quantity)
            .NotEmpty();
    }
}