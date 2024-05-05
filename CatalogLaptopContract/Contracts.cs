using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogLaptopContract
{
    public record CatalogLaptopCreated(Guid Id, string StoreID, string Name, string Description, decimal Price, int Quantity, Boolean isAvailable, List<string> Image);
    public record CatalogLaptopUpdated(Guid Id, string StoreID, string Name, string Description, decimal Price, int Quantity, Boolean isAvailable, List<string> Image);
    public record CataloglaptopDeleted(Guid Id);
}
