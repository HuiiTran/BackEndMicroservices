using Coupons.Dtos;
using Coupons.Entities;

namespace Coupons
{
    public static class Extension
    {
        public static CouponDto AsDto(this Coupon coupon)
        {
            return new CouponDto(coupon.Id, coupon.Code, coupon.Name, coupon.Description, coupon.DiscountAmount, coupon.StartedDate, coupon.ExpiredDate);
        }
    }
}
