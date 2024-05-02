using Cart.Dto;
using Cart.Entities;
using Microsoft.AspNetCore.Mvc;
using ServicesCommon;

namespace Cart.Controllers
{
    [ApiController]
    [Route("cart")]
    public class CartController : ControllerBase
    {
        private readonly IRepository<CartItem> _itemsRepository;
        private readonly IRepository<CatalogItem> _catalogItemsRepository;
        public CartController(IRepository<CartItem> itemsRepository, IRepository<CatalogItem> catalogItemsRepository)
        {
            this._itemsRepository = itemsRepository;
            this._catalogItemsRepository = catalogItemsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }
            var cartItemEntities = await _itemsRepository.GetAllAsync(item=> item.UserId == userId);
            var itemIds = cartItemEntities.Select(item => item.CatalogLaptopId);
            var catalogItemEntites = await _catalogItemsRepository.GetAllAsync(item => itemIds.Contains(item.Id));

            var cartItemDto = cartItemEntities.Select(cartItem =>
            {
                var catalogItem = catalogItemEntites.Single(catalogItem => catalogItem.Id == cartItem.CatalogLaptopId);

                return cartItem.AsDto(catalogItem.Name, catalogItem.Description, catalogItem.Price, catalogItem.Image);
            });
            return Ok(cartItemDto);
        }
    }
}
