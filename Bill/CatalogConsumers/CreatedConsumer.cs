using MassTransit;
using CatalogLaptopContract;
using ServicesCommon;
using Bill.Entities;

namespace Cart.CatalogConsumers
{
    public class CreatedConsumer : IConsumer<CatalogLaptopCreated>
    {
        public readonly IRepository<CatalogItem> repository;

        public CreatedConsumer(IRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }
        public async Task Consume(ConsumeContext<CatalogLaptopCreated> context)
        {
            var message = context.Message;

            var laptop = await repository.GetAsync(message.Id);

            if (laptop != null)
            {
                return;
            }

            laptop = new CatalogItem
            {
                Id = message.Id,
                Name = message.Name,
                Price = message.Price,
                Image = message.Image,
                Quantity = message.Quantity,
            };

            await repository.CreateAsync(laptop);
        }
    }
}
