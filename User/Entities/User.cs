using ServicesCommon;

namespace User.Entities
{
    public class User : IEntity
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string PassWord { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

    }
}
