using E_commerce.application.Dtos.Requests;
using FluentValidation;

namespace E_commerce.Validatos
{
    public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
    {
        public OrderItemRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId inválido");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity debe ser al menos 1");
        }
    }
}
