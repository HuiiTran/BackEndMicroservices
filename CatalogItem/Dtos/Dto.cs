using System.ComponentModel.DataAnnotations;

namespace CatalogItem.Dtos
{
    public record LaptopDto(Guid Id, string StoreID, string Classify, string Name, string Description, decimal Price, int Quantity, Boolean isAvailable, List<string> Image, int SoldQuantity);

    public record CreateLaptopDto([Required]string Name, [Required]string StoreID, string Classify, string Description, decimal Price, int Quantity, Boolean isAvailable, List<IFormFile> Image);

    public record UpdateLaptopDto([Required]string Name, [Required]string StoreID, string Classify, string Description, decimal Price, int Quantity, Boolean isAvailable, List<IFormFile>? Image);

}

