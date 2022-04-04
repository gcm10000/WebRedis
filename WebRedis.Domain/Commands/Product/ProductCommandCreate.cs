using WebRedis.Domain.Commands.Product.Validations;

namespace WebRedis.Domain.Commands.Product
{
    public class ProductCommandCreate : ProductCommand
    {
        public ProductCommandCreate()
        {
            
        }

        public override bool IsValid()
        {
            ValidationResult = new ProductValidatorCreate().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
