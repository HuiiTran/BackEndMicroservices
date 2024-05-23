using AuthService.Entities;
using MassTransit;
using ServicesCommon;
using UserContract;

namespace AuthService.Consumer.UserConsumer
{
    public class UserDeleteConsumer : IConsumer<UserDeleted>
    {
        private readonly IRepository<AllUser> repository;

        public UserDeleteConsumer(IRepository<AllUser> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<UserDeleted> context)
        {
            var message = context.Message;

            var allUser = await repository.GetAsync(message.Id);

            if (allUser == null)
            {
                return;
            }

            await repository.RemoveAsync(message.Id);
        }
    }
}
