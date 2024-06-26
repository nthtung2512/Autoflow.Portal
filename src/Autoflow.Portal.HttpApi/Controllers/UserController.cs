using Autoflow.Portal.Application.Contracts.Repositories;
using Autoflow.Portal.Domain.ChatBox;
using Autoflow.Portal.Domain.ChatBoxDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Autoflow.Portal.HttpApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController(IUserRepository userRepository) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var userDtos = users.Select(user => new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Password = ""
            });
            return Ok(userDtos);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserById(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Password = ""
            };
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User(userDTO.Id) { Username = userDTO.Username, Password = userDTO.Password };
            await _userRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { userId = user.Id }, userDTO);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UserDTO userDTO)
        {
            if (userId != userDTO.Id)
            {
                return BadRequest("User ID mismatch");
            }

            var user = new User(userDTO.Id) { Username = userDTO.Username, Password = userDTO.Password };
            await _userRepository.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            if (await _userRepository.GetUserByIdAsync(userId) == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteUserAsync(userId);
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] UserLoginRequestDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Invalid client request");
            }

            var users = await _userRepository.GetAllUsersAsync();
            var user = users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }

            var userResponse = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Password = ""
            };

            return Ok(userResponse);
        }
    }
}
