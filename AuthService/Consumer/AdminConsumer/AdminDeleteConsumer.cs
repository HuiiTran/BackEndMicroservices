using AdminContract;
using AuthService.Entities;
using MassTransit;
using ServicesCommon;

namespace AuthService.Consumer.AdminConsumer
{
    public class AdminDeleteConsumer : IConsumer<AdminDeleted>
    {
        private readonly IRepository<AllUser> repository;

        public AdminDeleteConsumer(IRepository<AllUser> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<AdminDeleted> context)
        {
            var message = context.Message;

            var alluser= await repository.GetAsync(message.Id);

            if (alluser == null) {
                return;
            }

            await repository.RemoveAsync(message.Id);
        }
    }
}
