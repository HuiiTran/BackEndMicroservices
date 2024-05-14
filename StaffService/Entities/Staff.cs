using ServicesCommon;

namespace StaffService.Entities
{
    public class Staff : IEntity
    {
        public Guid Id { get; set; }

        public required string UserName { get; set; }

        public required string PassWord { get; set; }

        public required string Email { get; set; }

        public string? Address { get; set; }

        public string? Name { get; set; }

        public required string PhoneNumber { get; set; }

        public string? Image { get; set; }

        public decimal Salary { get; set; } = 0;

        public string Role = "Staff";



    }
}
