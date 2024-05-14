using ServicesCommon;

namespace AdminService.Entities
{
    public class Admin : IEntity
    {
        public Guid Id { get; set; }

        public required string UserName { get; set; }

        public required string PassWord { get; set; }

        public required string Email { get; set; }

        public string? Name { get; set; }

        public required string PhoneNumber { get; set; }

        public string? Image { get; set; }

        public string Role = "Admin";
    }
}
