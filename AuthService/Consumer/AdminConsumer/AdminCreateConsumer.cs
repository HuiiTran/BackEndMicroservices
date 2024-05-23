using AdminContract;
using AuthService.Entities;
using MassTransit;
using ServicesCommon;

namespace AuthService.Consumer.AdminConsumer
{
    public class AdminCreateConsumer : IConsumer<AdminCreated>
    {
        public readonly IRepository<AllUser> repository;

        public AdminCreateConsumer(IRepository<AllUser> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<AdminCreated> context)
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
                UserName = message.userName,
                Password = message.Password,
                Role = message.Role,
            };

            await repository.CreateAsync(allUser);
        }
    }
   
}
