using ServicesCommon;

namespace Bill.Entities
{
    public class Bills : IEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public List<Guid>? CatalogItemId { get; set; }

        public List<int> Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public string State { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
    }
}
