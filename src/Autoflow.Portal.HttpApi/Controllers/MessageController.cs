using Autoflow.Portal.Application.Contracts.Repositories;
using Autoflow.Portal.Domain.ChatBox;
using Autoflow.Portal.Domain.ChatBoxDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Autoflow.Portal.HttpApi.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessageController(IMessageRepository messageRepository) : ControllerBase
    {
        private readonly IMessageRepository _messageRepository = messageRepository;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetAllMessages()
        {
            var messages = await _messageRepository.GetAllMessagesAsync();
            var messageDtos = messages.Select(message => new MessageDTO
            {
                Id = message.Id,
                Content = message.Content,
                SendUserId = message.SendUserId,
                ReceiveUserId = message.ReceiveUserId,
                ConversationId = message.ConversationId
            });
            return Ok(messageDtos);
        }

        [HttpGet("{messageId}")]
        public async Task<ActionResult<MessageDTO>> GetMessageById(Guid messageId)
        {
            var message = await _messageRepository.GetMessageByIdAsync(messageId);
            if (message == null)
            {
                return NotFound();
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

        [HttpGet("conversation/{conversationId}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessagesForConversation(Guid conversationId)
        {
            var messages = await _messageRepository.GetMessagesForConversationAsync(conversationId);
            if (messages == null)
            {
                return NotFound();
            }

            var messageDtos = messages.Select(message => new MessageDTO
            {
                Id = message.Id,
                Content = message.Content,
                SendUserId = message.SendUserId,
                ReceiveUserId = message.ReceiveUserId,
                ConversationId = message.ConversationId
            });
            return Ok(messageDtos);
        }

        [HttpPost]
        public async Task<ActionResult<MessageDTO>> CreateMessage([FromBody] MessageDTO messageDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var message = new Message(messageDTO.Id) { Content = messageDTO.Content, SendUserId = messageDTO.SendUserId, ReceiveUserId = messageDTO.ReceiveUserId, ConversationId = messageDTO.ConversationId };
            await _messageRepository.AddMessageAsync(message);
            return CreatedAtAction(nameof(GetMessageById), new { messageId = message.Id }, messageDTO);
        }

        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeleteMessage(Guid messageId)
        {
            var message = await _messageRepository.GetMessageByIdAsync(messageId);
            if (message == null)
            {
                return NotFound();
            }

            await _messageRepository.DeleteMessageAsync(messageId);

            return NoContent();
        }
    }
}
