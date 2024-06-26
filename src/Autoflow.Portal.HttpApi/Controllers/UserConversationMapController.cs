using Autoflow.Portal.Application.Contracts.Repositories;
using Autoflow.Portal.Domain.ChatBox;
using Autoflow.Portal.Domain.ChatBoxDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Autoflow.Portal.HttpApi.Controllers
{
    [Route("api/userConversationMaps")]
    [ApiController]
    public class UserConversationMapController(IUserConversationMapRepository userConversationMapRepository) : ControllerBase
    {
        private readonly IUserConversationMapRepository _userConversationMapRepository = userConversationMapRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserConversationMapDTO>>> GetAllUserConversationMaps()
        {
            var userConversationMaps = await _userConversationMapRepository.GetAllUserConversationMaps();
            var userConversationMapDtos = userConversationMaps.Select(map => new UserConversationMapDTO
            {
                UserId = map.UserId,
                ConversationId = map.ConversationId
            });
            return Ok(userConversationMapDtos);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserConversationMapDTO>>> GetUserConversationMapsByUserId(Guid userId)
        {
            var userConversationMaps = await _userConversationMapRepository.GetMapByUserId(userId);
            var userConversationMapDtos = userConversationMaps.Select(map => new UserConversationMapDTO
            {
                UserId = map.UserId,
                ConversationId = map.ConversationId
            });
            return Ok(userConversationMapDtos);
        }

        [HttpGet("conversation/{conversationId}")]
        public async Task<ActionResult<IEnumerable<UserConversationMapDTO>>> GetUserConversationMapsByConversationId(Guid conversationId)
        {
            var userConversationMaps = await _userConversationMapRepository.GetMapByConversationId(conversationId);
            var userConversationMapDtos = userConversationMaps.Select(map => new UserConversationMapDTO
            {
                UserId = map.UserId,
                ConversationId = map.ConversationId
            });
            return Ok(userConversationMapDtos);
        }

        [HttpPost]
        public async Task<ActionResult<UserConversationMap>> AddUserConversationMap([FromBody] UserConversationMapDTO userConversationMapDTO)
        {
            var userConversationMap = new UserConversationMap(userConversationMapDTO.UserId, userConversationMapDTO.ConversationId);
            await _userConversationMapRepository.AddUserConversationMapAsync(userConversationMap);
            return CreatedAtAction(nameof(GetAllUserConversationMaps), userConversationMapDTO);
        }
    }
}
