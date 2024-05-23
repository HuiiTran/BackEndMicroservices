using ServicesCommon;

namespace AuthService.Entities
{
    public class AllUser : IEntity
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Role {  get; set; }
    }
}
