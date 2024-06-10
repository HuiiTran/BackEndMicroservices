using CatalogItem.Entities;
using MassTransit;
using ServicesCommon;
using SoldQuantityUpdate;


namespace CatalogItem.Consumer
{
    public class UpdateSoldQuantityConsumer : IConsumer<UpdateSoldQuantity>
    {
        private readonly IRepository<Laptop> repository;

        public UpdateSoldQuantityConsumer(IRepository<Laptop> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<UpdateSoldQuantity> context)
        {
            var message = context.Message;

            var laptop = await repository.GetAsync(message.Id);

            if (laptop == null)
            {

            }
            else
            {
                laptop.SoldQuantity = laptop.SoldQuantity + message.SoldQuantity;
                await repository.UpdateAsync(laptop);
            }
        }
    }
}
