using Cart.Entities;
using CatalogLaptopContract;
using MassTransit;
using ServicesCommon;

namespace Cart.CatalogConsumers
{
    public class DeleteConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IRepository<CatalogItem> repository;

        public DeleteConsumer(IRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }
        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            var message = context.Message;

            var item = await repository.GetAsync(message.Id);

            if (item == null)
            {
                return;
            }




            await repository.RemoveAsync(message.Id);
        }
    }
}
