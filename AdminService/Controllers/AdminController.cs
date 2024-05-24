using AdminContract;
using AdminService.Dto;
using AdminService.Entities;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ServicesCommon;

namespace AdminService.Controllers
{

    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly IRepository<Admin> adminRepository;
        private readonly IPublishEndpoint publishEndpoint;

        public AdminController(IRepository<Admin> adminRepository, IPublishEndpoint publishEndpoint)
        {
            this.adminRepository = adminRepository;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAsync()
        {
            var admins = (await adminRepository.GetAllAsync())
                .Select(admin => admin.AsDto());

            return Ok(admins);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdminDto>> GetByIdAsync(Guid id)
        {
            var admin = await adminRepository.GetAsync(id);
            if(admin == null)
            {
                return NotFound();
            }

            return admin.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<AdminDto>> PostAsync([FromForm] CreateAdminDto createAdminDto)
        {
            var admin = new Admin
            {
                UserName = createAdminDto.UserName,
                PassWord = createAdminDto.PassWord,
                Email = createAdminDto.Email,
                Name = createAdminDto.Name,
                PhoneNumber = createAdminDto.PhoneNumber,

            };

            if (createAdminDto.Image != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                createAdminDto.Image.OpenReadStream().CopyTo(memoryStream);
                admin.Image = Convert.ToBase64String(memoryStream.ToArray());
            }
            else
            {
                admin.Image = " ";
            }

            await adminRepository.CreateAsync(admin);

            await publishEndpoint.Publish(new AdminCreated(admin.Id, admin.UserName, admin.PassWord, admin.Role));

            return Ok(admin);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, [FromForm] UpdateAdminDto updateAdminDto)
        {
            var existingAdmin = await adminRepository.GetAsync(id);

            if (existingAdmin == null)
            {
                return NotFound();
            }

            existingAdmin.UserName = updateAdminDto.UserName;
            existingAdmin.PassWord = updateAdminDto.PassWord;
            existingAdmin.Email = updateAdminDto.Email;
            existingAdmin.Name = updateAdminDto.Name;
            existingAdmin.PhoneNumber = updateAdminDto.PhoneNumber;
            if(updateAdminDto.Image != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                updateAdminDto.Image.OpenReadStream().CopyTo(memoryStream);
                existingAdmin.Image = Convert.ToBase64String(memoryStream.ToArray()) ;
            }
            else
            {
                existingAdmin.Image = " ";
            }

            await adminRepository.UpdateAsync(existingAdmin);

            await publishEndpoint.Publish(new AdminUpdated(existingAdmin.Id, existingAdmin.UserName, existingAdmin.PassWord, existingAdmin.Role));

            return Ok(existingAdmin);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var existingAdmin = await adminRepository.GetAsync(id);
            if (existingAdmin == null)
                return NotFound();

            await adminRepository.RemoveAsync(existingAdmin.Id);

            await publishEndpoint.Publish(new AdminDeleted(existingAdmin.Id));

            return NoContent();
        }
    }
}
