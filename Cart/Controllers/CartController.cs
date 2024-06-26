﻿using Amazon.Runtime.Internal;
using Amazon.Util;
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
        private readonly IRepository<CartItem> _cartitemsRepository;
        private readonly IRepository<CatalogItem> _catalogItemsRepository;
        public CartController(IRepository<CartItem> CartitemsRepository, IRepository<CatalogItem> catalogItemsRepository)
        {
            this._cartitemsRepository = CartitemsRepository;
            this._catalogItemsRepository = catalogItemsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var cartItemEntities = await _cartitemsRepository.GetAllAsync(item=> item.UserId == userId);
            var itemIds = cartItemEntities.Select(item => item.CatalogLaptopId);
            var catalogItemEntites = await _catalogItemsRepository.GetAllAsync(item => itemIds.Contains(item.Id));

            var cartItemDto = cartItemEntities.Select(cartItem =>
            {
                var catalogItem = catalogItemEntites.Single(catalogItem => catalogItem.Id == cartItem.CatalogLaptopId);
                return cartItem.AsDto(catalogItem.Name, catalogItem.Description, catalogItem.Price, catalogItem.Image);
            });
            return Ok(cartItemDto);
        }


        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemDto grantItemDto)
        {
            var cartItem = await _cartitemsRepository.GetAsync(item =>
                item.UserId == grantItemDto.UserId && item.CatalogLaptopId == grantItemDto.CatalogLaptopId);
            var catalogItemEntites = await _catalogItemsRepository.GetAsync(grantItemDto.CatalogLaptopId);
            if (catalogItemEntites.Quantity >= grantItemDto.Quantity )
            {
                if (cartItem == null)
                {
                    cartItem = new CartItem
                    {
                        CatalogLaptopId = grantItemDto.CatalogLaptopId,
                        UserId = grantItemDto.UserId,
                        Quantity = grantItemDto.Quantity,
                    };
                    await _cartitemsRepository.CreateAsync(cartItem);
                }
                else
                {
                    if (catalogItemEntites.Quantity >= cartItem.Quantity + grantItemDto.Quantity)
                    {
                        cartItem.Quantity += grantItemDto.Quantity;
                        await _cartitemsRepository.UpdateAsync(cartItem);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            else
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(GrantItemDto grantItemDto)
        {
            var cartItem = await _cartitemsRepository.GetAsync(item =>
                item.UserId == grantItemDto.UserId && item.CatalogLaptopId == grantItemDto.CatalogLaptopId);

            if (cartItem == null)
            {
                return NotFound();
            }
            else if (cartItem.Quantity > 0)
            {
                cartItem.Quantity -= grantItemDto.Quantity;
                await _cartitemsRepository.UpdateAsync(cartItem);
            }

            if (cartItem.Quantity <= 0)
            {
                await _cartitemsRepository.RemoveAsync(cartItem.Id);
            }
            return Ok();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(GrantItemDto grantItemDto)
        {
            var cartItem = await _cartitemsRepository.GetAsync(item =>
                item.UserId == grantItemDto.UserId && item.CatalogLaptopId == grantItemDto.CatalogLaptopId);

            if(cartItem == null)
            {
                return NotFound();
            }
            
            await _cartitemsRepository.RemoveAsync(cartItem.Id);

            return Ok();
        }
    }
}
