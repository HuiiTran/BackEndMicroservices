using Cart.Dto;
using Cart.Entities;

namespace Cart
{
    public static class Extension
    {
        public static CartItemDto AsDto(this CartItem item, string name, string description, decimal price, string image )
        {
            return new CartItemDto(item.CatalogLaptopId, name, description, item.Quantity, price, image);
        }
    }
}
