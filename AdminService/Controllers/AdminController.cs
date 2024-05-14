using AdminService.Dto;
using AdminService.Entities;
using Microsoft.AspNetCore.Mvc;
using ServicesCommon;

namespace AdminService.Controllers
{

    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly IRepository<Admin> adminRepository;

        public AdminController(IRepository<Admin> adminRepository)
        {
            this.adminRepository = adminRepository;
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

            return Ok(existingAdmin);
        }
    }
}
