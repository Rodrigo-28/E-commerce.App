using E_commerce.application.Dtos.Requests;
using FluentValidation;

namespace E_commerce.Validatos
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId inválido");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Debe haber al menos un ítem en la orden");

            // Aplica el validator interno a cada elemento de la lista
            RuleForEach(x => x.Items)
                .SetValidator(new OrderItemRequestValidator());
        }
    }
}
