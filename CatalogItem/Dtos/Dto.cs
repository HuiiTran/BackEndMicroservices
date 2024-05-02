using System.ComponentModel.DataAnnotations;

namespace CatalogItem.Dtos
{
    public record LaptopDto(Guid Id, string StoreID, string Name, string Description, decimal Price, int Quantity, Boolean isAvailable, string Image);

    public record CreateLaptopDto([Required]string Name, [Required]string StoreID, string Description, decimal Price, int Quantity, Boolean isAvailable, IFormFile Image);

    public record UpdateLaptopDto([Required]string Name, [Required]string StoreID, string Description, decimal Price, int Quantity, Boolean isAvailable, IFormFile Image);

}

