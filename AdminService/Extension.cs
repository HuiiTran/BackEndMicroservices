using AdminService.Dto;
using AdminService.Entities;

namespace AdminService
{
    public static class Extension
    {
        public static AdminDto AsDto(this Admin admin)
        {
            return new AdminDto(admin.Id, admin.UserName, admin.PassWord, admin.Email, admin.Name, admin.PhoneNumber, admin.Image, admin.Role);
        }
    }
}
