namespace GPTLearning.Models
{
    public class Conversation
    {
        public Guid Id { get; set; }  // Unique ID for the conversation
        public DateTime StartTime { get; set; }  // Start time of the conversation
        public List<Message> Messages { get; set; }  // List to track messages exchanged

        public Conversation()
        {
            Id = Guid.NewGuid();
            StartTime = DateTime.UtcNow;
            Messages = new List<Message>();
        }
    }
}
