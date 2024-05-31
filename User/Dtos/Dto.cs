namespace User.Dtos
{
    public record UserDto(Guid Id, string UserName, string PassWord, string Email, string Address, string Name, string PhoneNumber, string Image, string Role);
    public record CreateUserDto(string UserName, string PassWord, string Email, string Address, string Name, string PhoneNumber, IFormFile Image);
    public record UpdateUserDto(string UserName, string PassWord, string Email, string Address, string Name, string PhoneNumber, IFormFile? Image);

}
