using BillUpdateCatalogItem;
using CatalogItem.Entities;
using MassTransit;
using ServicesCommon;

namespace CatalogItem.Consumer
{
    public class UpdateConsumer : IConsumer<BillCatalogItemUpdated>
    {
        private readonly IRepository<Laptop> repository;

        public UpdateConsumer(IRepository<Laptop> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<BillCatalogItemUpdated> context)
        {
            var message = context.Message;

            var laptop = await repository.GetAsync(message.Id);

            if ( laptop == null)
            {
                
            }
            else
            {
                laptop.Quantity = message.Quantity;

                await repository.UpdateAsync(laptop);
            }
        }
    }
}
