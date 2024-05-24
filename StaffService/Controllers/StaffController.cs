using Amazon.Runtime.Internal.Transform;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ServicesCommon;
using StaffContract;
using StaffService.Dto;
using StaffService.Entities;

namespace StaffService.Controllers
{
    [ApiController]
    [Route("staff")]
    public class StaffController : ControllerBase
    {
        private readonly IRepository<Staff> staffRepository;
        private readonly IPublishEndpoint publishEndpoint;

        public StaffController(IRepository<Staff> staffRepository, IPublishEndpoint publishEndpoint)
        {
            this.staffRepository = staffRepository;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Staff>>> GetAsync()
        {
            var staffs = (await staffRepository.GetAllAsync())
                .Select(user => user.AsDto());
            return Ok(staffs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StaffDto>> GetByIdAsync(Guid id)
        {
            var staff = await staffRepository.GetAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return staff.AsDto();
        }


        [HttpPost]
        public async Task<ActionResult<StaffDto>> PostAsync([FromForm] CreateStaffDto createStaffDto)
        {



            var staff = new Staff
            {
                UserName = createStaffDto.UserName,
                PassWord = createStaffDto.PassWord,
                Email = createStaffDto.Email,
                Address = createStaffDto.Address,
                Name = createStaffDto.Name,
                PhoneNumber = createStaffDto.PhoneNumber,
            };
            if (createStaffDto.Image != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                createStaffDto.Image.OpenReadStream().CopyTo(memoryStream);
                staff.Image = Convert.ToBase64String(memoryStream.ToArray());
            }
            else
            {
                staff.Image = " ";
            }
            await staffRepository.CreateAsync(staff);

            await publishEndpoint.Publish(new StaffCreated(staff.Id, staff.UserName, staff.PassWord, staff.Role));

            return Ok(staff);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, [FromForm] UpdateStaffDto updateStaffDto)
        {
            var existingStaff = await staffRepository.GetAsync(id);

            if (existingStaff == null)
            {
                return NotFound();
            }
            existingStaff.UserName = updateStaffDto.UserName;
            existingStaff.PassWord = updateStaffDto.PassWord;
            existingStaff.Email = updateStaffDto.Email;
            existingStaff.Address = updateStaffDto.Address;
            existingStaff.Name = updateStaffDto.Name;
            existingStaff.PhoneNumber = updateStaffDto.PhoneNumber;
            if (updateStaffDto.Image != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                updateStaffDto.Image.OpenReadStream().CopyTo(memoryStream);
                existingStaff.Image = Convert.ToBase64String(memoryStream.ToArray());
            }
            else
            {
                existingStaff.Image = " ";
            }

            await staffRepository.UpdateAsync(existingStaff);

            await publishEndpoint.Publish(new StaffUpdated(existingStaff.Id, existingStaff.UserName, existingStaff.PassWord, existingStaff.Role));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var existingStaff = await staffRepository.GetAsync(id);

            if (existingStaff == null)
            {
                return NotFound();
            }

            await staffRepository.RemoveAsync(existingStaff.Id);

            await publishEndpoint.Publish(new StaffDeleted(existingStaff.Id));

            return NoContent();
        }

    }
}
