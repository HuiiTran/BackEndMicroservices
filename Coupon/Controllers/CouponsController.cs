using Coupons.Dtos;
using Coupons.Entities;
using Microsoft.AspNetCore.Mvc;
using ServicesCommon;

namespace Coupons.Controllers
{
    [ApiController]
    [Route("coupons")]
    public class CouponsController : ControllerBase
    {
        private readonly IRepository<Coupon> couponRepository;

        public CouponsController(IRepository<Coupon> couponRepository)
        {
            this.couponRepository = couponRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CouponDto>>> GetAsync()
        {
            var coupons = (await couponRepository.GetAllAsync())
                .Select(coupon => coupon.AsDto());

            return Ok(coupons);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CouponDto>> GetByIdAsync(Guid id)
        {
            var coupon = await couponRepository.GetAsync(id);

            if (coupon == null)
            {
                return NotFound();
            }

            return coupon.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<CouponDto>> PostAsync(CreateCouponDto createCouponDto)
        {
            var coupon = new Coupon
            {
                Code = createCouponDto.Code,
                Name = createCouponDto.Name,
                Description = createCouponDto.Description,
                DiscountAmount = createCouponDto.DiscountAmount,
                StartedDate = createCouponDto.StartedDate,
                ExpiredDate = createCouponDto.ExpiredDate,
            };


            await couponRepository.CreateAsync(coupon);

            return Ok(coupon);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateCouponDto updateCouponDto)
        {
            var existingCoupon = await couponRepository.GetAsync(id);

            if (existingCoupon == null)
            {
                return NotFound();
            }

            existingCoupon.Code = updateCouponDto.Code;
            existingCoupon.Name = updateCouponDto.Name;
            existingCoupon.Description = updateCouponDto.Description;
            existingCoupon.DiscountAmount = updateCouponDto.DiscountAmount;
            existingCoupon.StartedDate = updateCouponDto.StartedDate;
            existingCoupon.ExpiredDate = updateCouponDto.ExpiredDate;



            await couponRepository.UpdateAsync(existingCoupon);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var coupon = await couponRepository.GetAsync(id);

            if (coupon == null)
            {
                return NotFound();
            }

            await couponRepository.RemoveAsync(coupon.Id);

            return NoContent();
        }
    }
}
