using Bill.Entities;

namespace Bill.Dto
{
    public record GrantItemDto(Guid UserId, List<Guid> CatalogItemId, List<int> Quantity, string State, string Address, string Phone);//add to bill
    public record BillItemDto(List<CatalogItem> catalogItems, List<int> Quantity, decimal TotalPrice, DateTimeOffset CreatedDate, Guid UserId, Guid Id, string State, string Address, string Phone);
    public record CatalogItemDto(Guid Id, string Name, decimal Price, string Image);
    public record UpdateBillDto(string State);
    /*public record AddOneItem(Guid Id);*/
}
