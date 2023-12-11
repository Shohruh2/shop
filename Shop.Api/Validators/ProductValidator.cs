using System.Data;
using FluentValidation;
using Shop.Api.Models;
using Shop.Api.Repositories;

namespace Shop.Api.Validators;

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