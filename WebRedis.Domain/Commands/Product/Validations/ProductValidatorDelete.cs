using FluentValidation;
using System;

namespace WebRedis.Domain.Commands.Product.Validations
{
    public class ProductValidatorDelete : AbstractValidator<ProductCommandDelete>
    {
        public ProductValidatorDelete()
        {
            this.ValidateId();
        }

        private void ValidateId()
        {
            RuleFor(c => c.Id)
            .Must(guid => guid != Guid.Empty)
            .WithMessage("Id vazio.");
        }
    }
}
