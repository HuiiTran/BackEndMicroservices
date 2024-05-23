using AdminContract;
using AuthService.Entities;
using MassTransit;
using ServicesCommon;

namespace AuthService.Consumer.AdminConsumer
{
    public class AdminUpdateConsumer : IConsumer<AdminUpdated>
    {
        private readonly IRepository<AllUser> repository;

        public AdminUpdateConsumer(IRepository<AllUser> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<AdminUpdated> context)
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
