using Bill.Entities;
using CatalogLaptopContract;
using MassTransit;
using ServicesCommon;

namespace Cart.CatalogConsumers
{
    public class UpdateConsumer : IConsumer<CatalogLaptopUpdated>
    {
        private readonly IRepository<CatalogItem> _catalogItemRepository;

        public UpdateConsumer(IRepository<CatalogItem> catalogItemRepository)
        { 
            _catalogItemRepository = catalogItemRepository;
        }
        public async Task Consume(ConsumeContext<CatalogLaptopUpdated> context)
        {
            var message = context.Message;

            var laptop = await _catalogItemRepository.GetAsync(message.Id);

            if (laptop == null)
            {
                laptop = new CatalogItem
                {
                    Id = message.Id,
                    Name = message.Name,
                    Price = message.Price,
                    Image = message.Image,
                    Quantity = message.Quantity,

                };
                await _catalogItemRepository.CreateAsync(laptop);
            }
            else
            {
                laptop.Name = message.Name;
                laptop.Price = message.Price;
                laptop.Image = message.Image;
                laptop.Quantity = message.Quantity;


                await _catalogItemRepository.UpdateAsync(laptop);
            }
        }
    }
}
