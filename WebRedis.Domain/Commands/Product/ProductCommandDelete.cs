using System;
using WebRedis.Domain.Commands.Product.Validations;

namespace WebRedis.Domain.Commands.Product
{
    public class ProductCommandDelete : ProductCommand
    {
        public ProductCommandDelete()
        {

        }

        public override bool IsValid()
        {
            ValidationResult = new ProductValidatorDelete().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
