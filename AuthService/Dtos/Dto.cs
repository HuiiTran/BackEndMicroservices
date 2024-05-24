namespace AuthService.Dtos
{
    public record userDto(Guid Id, string UserName, string PassWord, string Role);
}
