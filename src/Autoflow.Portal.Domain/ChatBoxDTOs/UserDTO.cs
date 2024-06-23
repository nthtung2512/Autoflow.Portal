using System.ComponentModel.DataAnnotations;

namespace Autoflow.Portal.Domain.ChatBoxDTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string Password { get; set; } = string.Empty;
    }
}
