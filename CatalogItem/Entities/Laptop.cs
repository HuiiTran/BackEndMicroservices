using ServicesCommon;

namespace CatalogItem.Entities
{
    public class Laptop : IEntity
    {
        public Guid Id { get; set; }

        public string? StoreID { get; set; }

        public string Classify {  get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public Boolean isAvailable { get; set; }

        public List<string>? Image {  get; set; }
    }
}
