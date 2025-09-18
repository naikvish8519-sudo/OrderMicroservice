using FluentValidation;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;

public class PizzaOrderAddRequestValidator : AbstractValidator<PizzaOrderAddRequest>
{
    public PizzaOrderAddRequestValidator()
    {
        RuleFor(x => x.PizzaSize).NotEmpty().WithMessage("Pizza name is required.");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("Unit price must be greater than 0.");
    }
}
