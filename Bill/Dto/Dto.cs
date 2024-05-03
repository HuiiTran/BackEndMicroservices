using Bill.Entities;

namespace Bill.Dto
{
    public record GrantItemDto(Guid UserId, List<Guid> CatalogItemId);//add to bill
    public record BillItemDto(List<CatalogItem> catalogItems, decimal TotalPrice);
    public record CatalogItemDto(Guid Id, string Name, int Quantity, decimal Price, string Image);
}
