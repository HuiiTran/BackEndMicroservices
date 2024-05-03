﻿using CatalogItem.Dtos;
using CatalogItem.Entities;
using CatalogLaptopContract;
using MassTransit;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Mvc;
using ServicesCommon;

namespace CatalogItem.Controllers
{
    [ApiController]
    [Route("laptops")]
    public class LaptopControllers : ControllerBase
    {
        private readonly IRepository<Laptop> laptopRepository;

        private readonly IPublishEndpoint publishEndpoint;

        public LaptopControllers(IRepository<Laptop> laptopRepository, IPublishEndpoint publishEndpoint)
        {
            this.laptopRepository = laptopRepository;
            this.publishEndpoint = publishEndpoint;
    }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LaptopDto>>> GetAsync()
        {
            var laptops = (await laptopRepository.GetAllAsync())
                .Select(laptop => laptop.AsDto());
            return Ok(laptops);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LaptopDto>> GetByIdAsync(Guid id)
        {
            var laptop = await laptopRepository.GetAsync(id);
            if (laptop == null)
            {
                return NotFound();
            }
            return laptop.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<LaptopDto>> PostAsync([FromForm]CreateLaptopDto createLaptopDto)
        {
            var laptop = new Laptop();
            laptop.StoreID = createLaptopDto.StoreID;
            laptop.Name = createLaptopDto.Name;
            laptop.Description = createLaptopDto.Description;
            laptop.Price = createLaptopDto.Price;
            laptop.Quantity = createLaptopDto.Quantity;
            laptop.isAvailable = createLaptopDto.isAvailable;
            if (createLaptopDto.Image != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                createLaptopDto.Image.OpenReadStream().CopyTo(memoryStream);
                laptop.Image = Convert.ToBase64String(memoryStream.ToArray());
            }
            else
            {
                laptop.Image = "";
            }
            
            await laptopRepository.CreateAsync(laptop);

            await publishEndpoint.Publish(new CatalogLaptopCreated(laptop.Id, laptop.StoreID, laptop.Name, laptop.Description, laptop.Price, laptop.Quantity, laptop.isAvailable, laptop.Image));

            //return CreatedAtAction(nameof(PostAsync), new {id = laptop.Id}, laptop);
            return Ok(laptop);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, [FromForm] UpdateLaptopDto updateLaptopDto)
        {
            var existingLatop = await laptopRepository.GetAsync(id);

            if (existingLatop == null)
            {
                return NotFound();
            }

            existingLatop.StoreID = updateLaptopDto.StoreID;
            existingLatop.Name = updateLaptopDto.Name;
            existingLatop.Description = updateLaptopDto.Description;
            existingLatop.Price = updateLaptopDto.Price;
            existingLatop.Quantity = updateLaptopDto.Quantity;
            existingLatop.isAvailable = updateLaptopDto.isAvailable;
            if (updateLaptopDto.Image != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                updateLaptopDto.Image.OpenReadStream().CopyTo(memoryStream);
                existingLatop.Image = Convert.ToBase64String(memoryStream.ToArray());
            }
            await laptopRepository.UpdateAsync(existingLatop);

            await publishEndpoint.Publish(new CatalogLaptopUpdated(existingLatop.Id, existingLatop.StoreID,existingLatop.Name, existingLatop.Description,existingLatop.Price,existingLatop.Quantity,existingLatop.isAvailable ,existingLatop.Image));
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAysnc(Guid id)
        {
            var laptop = await laptopRepository.GetAsync(id);

            if (laptop == null)
            {
                return NotFound();
            }

            await laptopRepository.RemoveAsync(laptop.Id);

            await publishEndpoint.Publish(new CatalogItemDeleted(laptop.Id));

            return NoContent();
        }
    }

    
}