using MassTransit;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Mvc;
using ServicesCommon;
using System.Net;
using System.Xml.Linq;
using User.Dtos;
using User.Entities;
using UserContract;

namespace User.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserControllers : ControllerBase
    {
        private readonly IRepository<Users> UserRepository;
        private readonly IPublishEndpoint publishEndpoint;

        public UserControllers(IRepository<Users> UserRepository, IPublishEndpoint publishEndpoint)
        {
            this.UserRepository = UserRepository;
            this.publishEndpoint = publishEndpoint;
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
           // Console.WriteLine(createUserDto.Image);
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

            await publishEndpoint.Publish(new UserCreated(user.Id, user.UserName, user.PassWord, user.Role));

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id,[FromForm] UpdateUserDto updateUserDto)
        {
            var existingUser = await UserRepository.GetAsync(id);
            var existingImage = existingUser.Image;
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
                existingUser.Image = existingImage;
            }


            await UserRepository.UpdateAsync(existingUser);

            await publishEndpoint.Publish(new UserUpdated(existingUser.Id, existingUser.UserName, existingUser.PassWord, existingUser.Role));


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

            await publishEndpoint.Publish(new UserDeleted(user.Id));


            return NoContent();
        }

    }
}
