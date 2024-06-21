using Autoflow.Portal.Application.Contracts.Repositories;
using Autoflow.Portal.Domain.ChatBox;
using Microsoft.AspNetCore.Mvc;


// Controller is where define CRUD API endpoints for the ChatBox domain
namespace Autoflow.Portal.HttpApi.Controllers
{
    [Route("api/chatbox")] // Route prefix for all endpoints in this controller
    [ApiController]
    public class ChatBoxController : ControllerBase
    {
        private readonly IChatBoxRepository _chatBoxRepository;

        public ChatBoxController(IChatBoxRepository chatBoxRepository)
        {
            _chatBoxRepository = chatBoxRepository;
        }

        // GET api/chatbox/users
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _chatBoxRepository.GetAllUsersAsync();
            return Ok(users);
        }

        // GET api/chatbox/users/{userId}
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<User>> GetUserById(Guid userId)
        {
            var user = await _chatBoxRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound(); // Return 404 Not Found if user with specified ID is not found
            }
            return Ok(user);
        }

        // POST api/chatbox/users
        [HttpPost("users")]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            await _chatBoxRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { userId = user.Id }, user);
        }

        // PUT api/chatbox/users/{userId}
        [HttpPut("users/{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] User user)
        {
            if (userId != user.Id)
            {
                return BadRequest("User ID mismatch");
            }

            await _chatBoxRepository.UpdateUserAsync(user);
            return NoContent(); // HTTP 204 No Content
        }

        // DELETE api/chatbox/users/{userId}
        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            await _chatBoxRepository.DeleteUserAsync(userId);
            return NoContent(); // HTTP 204 No Content
        }

        // GET api/chatbox/conversations
        [HttpGet("conversations")]
        public async Task<ActionResult<IEnumerable<Conversation>>> GetAllConversations()
        {
            var conversations = await _chatBoxRepository.GetAllConversationsAsync();
            return Ok(conversations);
        }

        // GET api/chatbox/conversations/{conversationId}
        [HttpGet("conversations/{conversationId}")]
        public async Task<ActionResult<Conversation>> GetConversationById(Guid conversationId)
        {
            var conversation = await _chatBoxRepository.GetConversationByIdAsync(conversationId);
            if (conversation == null)
            {
                return NotFound(); // Return 404 Not Found if conversation with specified ID is not found
            }
            return Ok(conversation);
        }

        // POST api/chatbox/conversations
        [HttpPost("conversations")]
        public async Task<ActionResult<Conversation>> CreateConversation([FromBody] Conversation conversation)
        {
            await _chatBoxRepository.AddConversationAsync(conversation);
            return CreatedAtAction(nameof(GetConversationById), new { conversationId = conversation.Id }, conversation);
        }

        // PUT api/chatbox/conversations/{conversationId}
        [HttpPut("conversations/{conversationId}")]
        public async Task<IActionResult> UpdateConversation(Guid conversationId, [FromBody] Conversation conversation)
        {
            if (conversationId != conversation.Id)
            {
                return BadRequest("Conversation ID mismatch");
            }

            await _chatBoxRepository.UpdateConversationAsync(conversation);
            return NoContent(); // HTTP 204 No Content
        }

        // Additional endpoints for messages and other functionalities can be added similarly

    }
}
