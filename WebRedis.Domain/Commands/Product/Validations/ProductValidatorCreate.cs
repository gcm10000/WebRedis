using FluentValidation;

namespace WebRedis.Domain.Commands.Product.Validations
{
    public class ProductValidatorCreate : AbstractValidator<ProductCommandCreate>
    {
        public ProductValidatorCreate()
        {
            this.ValidateProductName();
        }

        void ValidateProductName()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Nome do produto deve estar preenchido.");
        }
    }
}
