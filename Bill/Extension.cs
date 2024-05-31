using Bill.Dto;
using Bill.Entities;

namespace Bill
{
    public static class Extension
    {
        public static BillItemDto AsDto(this Bills bills, List<CatalogItem> catalogItems)
        {
            return new BillItemDto( catalogItems, bills.Quantity, bills.TotalPrice, bills.CreatedDate, bills.UserId, bills.Id, bills.State);
        }
    }
}
