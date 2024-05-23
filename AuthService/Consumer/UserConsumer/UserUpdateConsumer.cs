using AuthService.Entities;
using MassTransit;
using ServicesCommon;
using UserContract;

namespace AuthService.Consumer.UserConsumer
{
    public class UserUpdateConsumer : IConsumer<UserUpdated>
    {
        private readonly IRepository<AllUser> repository;

        public UserUpdateConsumer(IRepository<AllUser> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<UserUpdated> context)
        {
            var message = context.Message;

            var allUser = await repository.GetAsync(message.Id);

            if (allUser == null)
            {
                allUser = new AllUser
                {
                    Id = message.Id,
                    UserName = message.UserName,
                    Password = message.Password,
                    Role = message.Role,
                };
                await repository.CreateAsync(allUser);
            }
            else
            {
                allUser.Id = message.Id;
                allUser.UserName = message.UserName;
                allUser.Password = message.Password;
                allUser.Role = message.Role;

                await repository.UpdateAsync(allUser);
            }
        }
    }
}
