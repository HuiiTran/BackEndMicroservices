using AuthService.Entities;
using MassTransit;
using ServicesCommon;
using StaffContract;

namespace AuthService.Consumer.StaffConsumer
{
    public class StaffDeleteConsumer : IConsumer<StaffDeleted>
    {
        private readonly IRepository<AllUser> repository;

        public StaffDeleteConsumer(IRepository<AllUser> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<StaffDeleted> context)
        {
            var message = context.Message;

            var alluser = await repository.GetAsync(message.Id);

            if (alluser == null)
            {
                return;
            }

            await repository.RemoveAsync(message.Id);
        }
    }
}
