namespace UserContract
{
    public record UserCreated(Guid Id, string UserName, string Password, string Role);
    public record UserUpdated(Guid Id, string UserName, string Password, string Role);
    public record UserDeleted(Guid Id);
}
