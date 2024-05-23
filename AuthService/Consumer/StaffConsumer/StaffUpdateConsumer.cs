using AuthService.Entities;
using MassTransit;
using ServicesCommon;
using StaffContract;

namespace AuthService.Consumer.StaffConsumer
{
    public class StaffUpdateConsumer : IConsumer<StaffUpdated>
    {
        private readonly IRepository<AllUser> repository;

        public StaffUpdateConsumer(IRepository<AllUser> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<StaffUpdated> context)
        {
            var message = context.Message;


            var alluser = await repository.GetAsync(message.Id);

            if (alluser == null)
            {
                alluser = new AllUser
                {
                    Id = message.Id,
                    UserName = message.userName,
                    Password = message.Password,
                    Role = message.Role,
                };
                await repository.CreateAsync(alluser);
            }
            else
            {
                alluser.Id = message.Id;
                alluser.UserName = message.userName;
                alluser.Password = message.Password;
                alluser.Role = message.Role;

                await repository.UpdateAsync(alluser);
            }
        }
    }
}
