using System.ComponentModel.DataAnnotations;

namespace Autoflow.Portal.Domain.ChatBoxDTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        [StringLength(32, MinimumLength = 4)]
        public required string Username { get; set; } = string.Empty;

        [StringLength(16, MinimumLength = 4)]
        public required string Password { get; set; } = string.Empty;
    }
}
