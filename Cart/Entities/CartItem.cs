using ServicesCommon;

namespace Cart.Entities
{
    public class CartItem : IEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid CatalogLaptopId { get; set; }

        public int Quantity { get; set; }

        /*public string Image {  get; set; }

        public decimal Price { get; set; }*/
    }
}
