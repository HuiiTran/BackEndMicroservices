using CatalogItem.Dtos;
using CatalogItem.Entities;

namespace CatalogItem
{
    public static class Extensions
    {
        public static LaptopDto AsDto(this Laptop laptop)
        {
            return new LaptopDto(laptop.Id, laptop.StoreID, laptop.Name, laptop.Description, laptop.Price, laptop.Quantity, laptop.isAvailable, laptop.Image);
        }
    }
}
