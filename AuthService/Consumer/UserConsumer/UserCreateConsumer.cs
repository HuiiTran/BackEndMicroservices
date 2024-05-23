using AuthService.Entities;
using MassTransit;
using ServicesCommon;
using UserContract;

namespace AuthService.Consumer.UserConsumer
{
    public class UserCreateConsumer : IConsumer<UserCreated>
    {
        public readonly IRepository<AllUser> repository;

        public UserCreateConsumer(IRepository<AllUser> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<UserCreated> context)
        {
            var message = context.Message;

            var allUser = await repository.GetAsync(message.Id);

            if (allUser == null)
            {
                return;
            }

            allUser = new AllUser
            {
                Id = message.Id,
                UserName = message.UserName,
                Password = message.Password,
                Role = message.Role,
            };

            await repository.CreateAsync(allUser);
        }
    }
}
