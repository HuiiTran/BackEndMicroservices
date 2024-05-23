namespace AdminContract
{
    public record AdminCreated(Guid Id, string userName, string Password, string Role);
    public record AdminUpdated(Guid Id, string userName, string Password, string Role);
    public record AdminDeleted(Guid Id);
}
