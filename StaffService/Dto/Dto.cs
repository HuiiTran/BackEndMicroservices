namespace StaffService.Dto
{
    public record StaffDto(Guid Id, string UserName, string PassWord, string Email, string Address, string Name, string PhoneNumber, string Image, decimal Salary);
    public record CreateStaffDto(string UserName, string PassWord, string Email, string Address, string Name, string PhoneNumber, IFormFile Image, decimal Salary);
    public record UpdateStaffDto(string UserName, string PassWord, string Email, string Address, string Name, string PhoneNumber, IFormFile Image, decimal Salary);
}
