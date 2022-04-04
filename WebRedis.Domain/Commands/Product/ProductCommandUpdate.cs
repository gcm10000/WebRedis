namespace WebRedis.Domain.Commands.Product
{
    public class ProductCommandUpdate : Command
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
    }
}
