using Autoflow.Portal.Application.Contracts.Repositories;
using Autoflow.Portal.Domain.ChatBox;
using Autoflow.Portal.Domain.ChatBoxDTOs;
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
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _chatBoxRepository.GetAllUsersAsync();

            var userDtos = users.Select(user => new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Password = ""
            });

            return Ok(userDtos);
        }

        // GET api/chatbox/users/{userId}
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserById(Guid userId)
        {
            var user = await _chatBoxRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound(); // Return 404 Not Found if user with specified ID is not found
            }
            var userDto = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Password = ""
            };
            return Ok(userDto);
        }

        // POST api/chatbox/users
        [HttpPost("users")]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserDTO userDTO)
        {
            //checks whether the incoming data(in this case, userDto) passed from the client(frontend) adheres to the validation rules specified in the server-side model(in UserDTO class).
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User(userDTO.Id, userDTO.Username, userDTO.Password);

            await _chatBoxRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { userId = user.Id }, userDTO);
        }

        // PUT api/chatbox/users/{userId}
        [HttpPut("users/{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UserDTO userDTO)
        {
            if (userId != userDTO.Id)
            {
                return BadRequest("User ID mismatch");
            }

            var user = new User(userDTO.Id, userDTO.Username, userDTO.Password);

            await _chatBoxRepository.UpdateUserAsync(user);
            return NoContent(); // HTTP 204 No Content
        }

        // DELETE api/chatbox/users/{userId}
        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            if (await _chatBoxRepository.GetUserByIdAsync(userId) == null)
            {
                return NotFound(); // Return 404 Not Found if user with specified ID is not found
            }
            await _chatBoxRepository.DeleteUserAsync(userId);
            return NoContent(); // HTTP 204 No Content
        }

        // GET api/chatbox/messages/{messageId}
        [HttpGet("messages/{messageId}")]
        public async Task<ActionResult<MessageDTO>> GetMessageById(Guid messageId)
        {
            var message = await _chatBoxRepository.GetMessageByIdAsync(messageId);
            if (message == null)
            {
                return NotFound(); // Return 404 Not Found if message with specified ID is not found
            }
            var messageDto = new MessageDTO
            {
                Id = message.Id,
                Content = message.Content,
                SendUserId = message.SendUserId,
                ReceiveUserId = message.ReceiveUserId,
                ConversationId = message.ConversationId
            };
            return Ok(messageDto);
        }

        // GET api/chatbox/messages/conversation/{conversationId}
        [HttpGet("messages/conversation/{conversationId}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessagesForConversation(Guid conversationId)
        {
            var messages = await _chatBoxRepository.GetMessagesForConversationAsync(conversationId);
            if (messages == null)
            {
                return NotFound(); // Return 404 Not Found if no messages are found for the specified conversation
            }
            var messagesDto = messages.Select(message => new MessageDTO
            {
                Id = message.Id,
                Content = message.Content,
                SendUserId = message.SendUserId,
                ReceiveUserId = message.ReceiveUserId,
                ConversationId = message.ConversationId
            });
            return Ok(messagesDto);
        }

        // POST api/chatbox/messages
        [HttpPost("messages")]
        public async Task<ActionResult<MessageDTO>> CreateMessage([FromBody] MessageDTO messageDTO)
        {
            var message = new Message(messageDTO.Id, messageDTO.Content, messageDTO.SendUserId, messageDTO.ReceiveUserId, messageDTO.ConversationId);
            await _chatBoxRepository.AddMessageAsync(message);
            return CreatedAtAction(nameof(GetMessageById), new { messageId = message.Id }, messageDTO);
        }

        // GET api/chatbox/conversations
        [HttpGet("conversations")]
        public async Task<ActionResult<IEnumerable<ConversationDTO>>> GetAllConversations()
        {
            var conversations = await _chatBoxRepository.GetAllConversationsAsync();
            var conversationDtos = conversations.Select(conversation => new ConversationDTO
            {
                Id = conversation.Id
            });
            return Ok(conversationDtos);
        }

        // GET api/chatbox/conversations/{conversationId}
        [HttpGet("conversations/{conversationId}")]
        public async Task<ActionResult<ConversationDTO>> GetConversationById(Guid conversationId)
        {
            var conversation = await _chatBoxRepository.GetConversationByIdAsync(conversationId);
            if (conversation == null)
            {
                return NotFound(); // Return 404 Not Found if conversation with specified ID is not found
            }
            var conversationDto = new ConversationDTO
            {
                Id = conversation.Id
            };
            return Ok(conversationDto);
        }

        // POST api/chatbox/conversations
        [HttpPost("conversations")]
        public async Task<ActionResult<ConversationDTO>> CreateConversation([FromBody] ConversationDTO conversationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var conversation = new Conversation(conversationDTO.Id);

            await _chatBoxRepository.AddConversationAsync(conversation);
            return CreatedAtAction(nameof(GetConversationById), new { conversationId = conversation.Id }, conversationDTO);
        }

        // PUT api/chatbox/conversations/{conversationId}
        [HttpPut("conversations/{conversationId}")]
        public async Task<IActionResult> UpdateConversation(Guid conversationId, [FromBody] ConversationDTO conversationDTO)
        {
            if (conversationId != conversationDTO.Id)
            {
                return BadRequest("Conversation ID mismatch");
            }

            var conversation = new Conversation(conversationDTO.Id);

            await _chatBoxRepository.UpdateConversationAsync(conversation);
            return NoContent(); // HTTP 204 No Content
        }

        // GET api/chatbox/userConversationMaps
        [HttpGet("userConversationMaps")]
        public async Task<ActionResult<IEnumerable<UserConversationMapDTO>>> GetAllUserConversationMaps()
        {
            var userConversationMaps = await _chatBoxRepository.GetAllUserConversationMaps();
            var userConversationMapDtos = userConversationMaps.Select(map => new UserConversationMapDTO
            {
                UserId = map.UserId,
                ConversationId = map.ConversationId
            });
            return Ok(userConversationMapDtos);
        }

        // GET api/chatbox/userConversationMaps/user/{userId}
        [HttpGet("userConversationMaps/user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserConversationMapDTO>>> GetUserConversationMapsByUserId(Guid userId)
        {
            var userConversationMaps = await _chatBoxRepository.GetMapByUserId(userId);
            var userConversationMapDtos = userConversationMaps.Select(map => new UserConversationMapDTO
            {
                UserId = map.UserId,
                ConversationId = map.ConversationId
            });
            return Ok(userConversationMapDtos);
        }

        // GET api/chatbox/userConversationMaps/conversation/{conversationId}
        [HttpGet("userConversationMaps/conversation/{conversationId}")]
        public async Task<ActionResult<IEnumerable<UserConversationMapDTO>>> GetUserConversationMapsByConversationId(Guid conversationId)
        {
            var userConversationMaps = await _chatBoxRepository.GetMapByConversationId(conversationId);
            var userConversationMapDtos = userConversationMaps.Select(map => new UserConversationMapDTO
            {
                UserId = map.UserId,
                ConversationId = map.ConversationId
            });
            return Ok(userConversationMapDtos);
        }


        // POST api/chatbox/userConversationMaps
        [HttpPost("userConversationMaps")]
        public async Task<ActionResult<UserConversationMap>> AddUserConversationMap([FromBody] UserConversationMapDTO userConversationMapDTO)
        {
            var userConversationMap = new UserConversationMap
            {
                UserId = userConversationMapDTO.UserId,
                ConversationId = userConversationMapDTO.ConversationId
            };
            await _chatBoxRepository.AddUserConversationMapAsync(userConversationMap);
            return CreatedAtAction(nameof(GetAllUserConversationMaps), userConversationMap);
        }

        // Additional endpoints for messages and other functionalities can be added similarly

    }
}
