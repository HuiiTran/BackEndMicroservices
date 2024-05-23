namespace StaffContract
{
    public class StaffContract
    {
        public record StaffCreated(Guid Id, string userName, string Password, string Role);
        public record StaffUpdated(Guid Id, string userName, string Password, string Role);
        public record StaffDeleted(Guid Id);
    }
}
