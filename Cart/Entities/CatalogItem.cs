using ServicesCommon;

namespace Cart.Entities
{
    public class CatalogItem : IEntity
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? Image { get; set; }
    }
}
