using User.Dtos;
using User.Entities;
namespace User
{
    public static class Extension
    {
        public static UserDto AsDto(this Users user)
        {
            return new UserDto(user.Id, user.UserName, user.PassWord, user.Email, user.Address, user.Name, user.PhoneNumber, user.Image, user.Role);
        }
    }
}
