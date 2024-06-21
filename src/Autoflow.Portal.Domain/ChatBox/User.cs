using Autoflow.Portal.Base.EFCore;
using System.ComponentModel.DataAnnotations;

namespace Autoflow.Portal.Domain.ChatBox
{
    public class User : Entity<Guid>
    {
        [Required]
        [StringLength(32, MinimumLength = 4)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [StringLength(16, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        public List<Message> SentMessages { get; set; } = new List<Message>();

        public List<Message> ReceivedMessages { get; set; } = new List<Message>();

        public bool VerifyPassword(string password)
        {
            return Password == password;
        }

        // Method to send a message
        public void SendMessage(User receiver, string content)
        {
            var message = new Message
            {
                Content = content,
                SendUser = this,
                ReceiveUser = receiver
            };

            // Add message to sender's and receiver's message logs
            SentMessages.Add(message);
            receiver.ReceivedMessages.Add(message);
        }

    }
}
