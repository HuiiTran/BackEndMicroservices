using StaffService.Dto;
using StaffService.Entities;

namespace StaffService
{
    public static class Extension
    {
        public static StaffDto AsDto(this Staff staff)
        {
            return new StaffDto(staff.Id, staff.UserName, staff.PassWord, staff.Email, staff.Address, staff.Name, staff.PhoneNumber, staff.Image, staff.Salary, staff.Role);
        }
    }
}
