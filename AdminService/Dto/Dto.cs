namespace AdminService.Dto
{
    public record AdminDto(Guid Id, string UserName, string PassWord, string Email, string Name, string PhoneNumber, string Image, string Role );
    public record CreateAdminDto(string UserName, string PassWord, string Email, string Name, string PhoneNumber, IFormFile Image );
    public record UpdateAdminDto(string UserName, string PassWord, string Email,  string Name, string PhoneNumber, IFormFile? Image );
}
