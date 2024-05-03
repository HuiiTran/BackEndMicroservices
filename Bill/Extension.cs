using Bill.Dto;
using Bill.Entities;

namespace Bill
{
    public static class Extension
    {
        public static BillItemDto AsDto(this Bills bills, decimal TotalPrice, List<CatalogItem> catalogItems)
        {
            return new BillItemDto( catalogItems, TotalPrice);
        }
    }
}
