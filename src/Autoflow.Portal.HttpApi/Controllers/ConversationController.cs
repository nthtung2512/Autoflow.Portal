using Autoflow.Portal.Application.Contracts.Repositories;
using Autoflow.Portal.Domain.ChatBox;
using Autoflow.Portal.Domain.ChatBoxDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Autoflow.Portal.HttpApi.Controllers
{
    [Route("api/conversations")]
    [ApiController]
    public class ConversationController(IConversationRepository conversationRepository) : ControllerBase
    {
        private readonly IConversationRepository _conversationRepository = conversationRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConversationDTO>>> GetAllConversations()
        {
            var conversations = await _conversationRepository.GetAllConversationsAsync();
            var conversationDtos = conversations.Select(conversation => new ConversationDTO
            {
                Id = conversation.Id
            });
            return Ok(conversationDtos);
        }

        [HttpGet("{conversationId}")]
        public async Task<ActionResult<ConversationDTO>> GetConversationById(Guid conversationId)
        {
            var conversation = await _conversationRepository.GetConversationByIdAsync(conversationId);
            if (conversation == null)
            {
                return NotFound();
            }

            var conversationDto = new ConversationDTO
            {
                Id = conversation.Id
            };
            return Ok(conversationDto);
        }

        [HttpPost]
        public async Task<ActionResult<ConversationDTO>> CreateConversation([FromBody] ConversationDTO conversationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var conversation = new Conversation(conversationDTO.Id);
            await _conversationRepository.AddConversationAsync(conversation);
            return CreatedAtAction(nameof(GetConversationById), new { conversationId = conversation.Id }, conversationDTO);
        }
    }
}
