using ServicesCommon;

namespace Coupons.Entities
{
    public class Coupon : IEntity
    {
        public Guid Id { get; set; }

        public required string Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int DiscountAmount { get; set; }

        public DateTime? StartedDate { get; set; }

        public DateTime? ExpiredDate { get; set; }
    }
}
