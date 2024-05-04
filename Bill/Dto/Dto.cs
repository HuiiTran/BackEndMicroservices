using Bill.Entities;

namespace Bill.Dto
{
    public record GrantItemDto(Guid UserId, List<Guid> CatalogItemId, List<int> Quantity);//add to bill
    public record BillItemDto(List<CatalogItem> catalogItems, List<int> Quantity, decimal TotalPrice);
    public record CatalogItemDto(Guid Id, string Name, decimal Price, string Image);
    public record UpdateBillDto(List<Guid> CatalogItemId, List<int> Quantity);
    
}
