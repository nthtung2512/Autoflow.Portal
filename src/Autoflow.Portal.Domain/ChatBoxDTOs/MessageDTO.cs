namespace Autoflow.Portal.Domain.ChatBoxDTOs
{
    public class MessageDTO
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid SendUserId { get; set; }
        public Guid ReceiveUserId { get; set; }
        public Guid ConversationId { get; set; }
    }
}
