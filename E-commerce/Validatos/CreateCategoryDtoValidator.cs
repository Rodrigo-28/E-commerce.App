using E_commerce.application.Dtos.Requests;
using FluentValidation;

namespace E_commerce.Validatos
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name es obligatorio")
                .MaximumLength(100);
        }
    }
}
