using AuthService.Entities;
using MassTransit;
using ServicesCommon;
using StaffContract;

namespace AuthService.Consumer.StaffConsumer
{
    public class StaffCreateConsumer : IConsumer<StaffCreated>
    {
        private readonly IRepository<AllUser> repository;

        public StaffCreateConsumer(IRepository<AllUser> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<StaffCreated> context)
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
