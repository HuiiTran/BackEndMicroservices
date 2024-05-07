using MassTransit.Initializers;
using Microsoft.AspNetCore.Mvc;
using ServicesCommon;
using System.Net;
using System.Xml.Linq;
using User.Dtos;
using User.Entities;

namespace User.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserControllers : ControllerBase
    {
        private readonly IRepository<Users> UserRepository;

        public UserControllers(IRepository<Users> UserRepository)
        {
            this.UserRepository = UserRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetAsync()
        {
            var users = (await UserRepository.GetAllAsync())
                .Select(user => user.AsDto());

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetByIdAsync(Guid id)
        {
            var user = await UserRepository.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> PostAsync( [FromForm]CreateUserDto createUserDto)
        {

            

            var user = new Users
            {
                UserName = createUserDto.UserName,
                PassWord = createUserDto.PassWord,
                Email = createUserDto.Email,
                Address = createUserDto.Address,
                Name = createUserDto.Name,
                PhoneNumber = createUserDto.PhoneNumber,
            };
            if (createUserDto.Image != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                createUserDto.Image.OpenReadStream().CopyTo(memoryStream);
                user.Image = Convert.ToBase64String(memoryStream.ToArray());
            }
            else
            {
                user.Image = " ";
            }
            await UserRepository.CreateAsync(user);

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id,[FromForm] UpdateUserDto updateUserDto)
        {
            var existingUser = await UserRepository.GetAsync(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.UserName = updateUserDto.UserName;
            existingUser.PassWord = updateUserDto.PassWord;
            existingUser.Email = updateUserDto.Email;
            existingUser.Address = updateUserDto.Address;
            existingUser.Name = updateUserDto.Name;
            existingUser.PhoneNumber = updateUserDto.PhoneNumber;
            if (updateUserDto.Image != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                updateUserDto.Image.OpenReadStream().CopyTo(memoryStream);
                existingUser.Image = Convert.ToBase64String(memoryStream.ToArray());
            }
            else
            {
                existingUser.Image = " ";
            }


            await UserRepository.UpdateAsync(existingUser);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var user = await UserRepository.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            await UserRepository.RemoveAsync(id);

            return NoContent();
        }

    }
}
