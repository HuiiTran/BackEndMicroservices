using CatalogItem.Dtos;
using CatalogItem.Entities;
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


        public LaptopControllers(IRepository<Laptop> laptopRepository)
        {
            this.laptopRepository = laptopRepository;
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

            return CreatedAtAction(nameof(PostAsync), new {id = laptop.Id}, laptop);
        }
    }

    
}
