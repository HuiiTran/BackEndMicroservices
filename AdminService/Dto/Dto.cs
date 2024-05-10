namespace AdminService.Dto
{
    public record AdminDto(Guid Id, string UserName, string PassWord, string Email, string Address, string Name, string PhoneNumber, string Image, decimal Salary);
    public record CreateAdminDto(string UserName, string PassWord, string Email, string Address, string Name, string PhoneNumber, IFormFile Image, decimal Salary);
    public record UpdateAdminDto(string UserName, string PassWord, string Email, string Address, string Name, string PhoneNumber, IFormFile Image, decimal Salary);
}
