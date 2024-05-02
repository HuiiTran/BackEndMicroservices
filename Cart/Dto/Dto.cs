namespace Cart.Dto
{
    public record GrantItemDto(Guid UserId, Guid CatalogLaptopId, int Quantity);//add an laptop to cart
    public record CartItemDto(Guid CatalogLaptopId, string Name, string Description, int Quantity, decimal Price, string Image);
    public record CatalogItemDto(Guid Id, string Name, string Description, string Image, decimal Price);
}
