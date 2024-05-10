namespace Coupons.Dtos
{
    public record CouponDto(Guid Id, string Code, string Name, string Description, int DiscountAmount, DateTime StartedDate, DateTime ExpiredDate);
    public record CreateCouponDto(string Name, string Code, string Description, int DiscountAmount, DateTime StartedDate, DateTime ExpiredDate);
    public record UpdateCouponDto(string Name, string Code, string Description, int DiscountAmount, DateTime StartedDate, DateTime ExpiredDate);
}
