using E_commerce.application.Dtos.Requests;
using FluentValidation;

namespace E_commerce.Validatos
{
    public class ProductQueryDtoValidator : AbstractValidator<ProductQueryDto>
    {
        public ProductQueryDtoValidator()
        {
            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0).WithMessage("MinPrice no puede ser negativo");

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(x => x.MinPrice.Value)
                .When(x => x.MaxPrice.HasValue && x.MinPrice.HasValue)
                .WithMessage("MaxPrice debe ser ≥ MinPrice");

            RuleFor(x => x.SearchTerm)
                .MaximumLength(100).WithMessage("SearchTerm máximo 100 caracteres");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).When(x => x.CategoryId.HasValue)
                .WithMessage("CategoryId debe ser > 0");
        }
    }
}
