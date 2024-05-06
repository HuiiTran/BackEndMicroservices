namespace User.Dtos
{
    public record UserDto(Guid Id, string UserName, string PassWord, string Email, string Address, string Name, string PhoneNumber);
    public record CreateUserDto(string UserName, string PassWord, string Email, string Address, string Name, string PhoneNumber);
    public record UpdateUserDto(string UserName, string PassWord, string Email, string Address, string Name, string PhoneNumber);

}
