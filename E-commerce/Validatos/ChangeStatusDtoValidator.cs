using E_commerce.application.Dtos.Requests;
using FluentValidation;

namespace E_commerce.Validatos
{
    public class ChangeStatusDtoValidator : AbstractValidator<ChangeStatusDto>
    {
        public ChangeStatusDtoValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Status inválido");
        }
    }
}
