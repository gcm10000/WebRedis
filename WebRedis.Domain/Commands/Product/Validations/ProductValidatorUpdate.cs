using FluentValidation;

namespace WebRedis.Domain.Commands.Product.Validations
{
    public class ProductValidatorUpdate : AbstractValidator<ProductCommandUpdate>
    {
        public ProductValidatorUpdate()
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
