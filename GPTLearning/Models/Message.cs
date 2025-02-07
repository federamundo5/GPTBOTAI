namespace GPTLearning.Models
{
    public class Message
    {
        public string Sender { get; set; }  // User or bot
        public string Content { get; set; }  // The message content
        public DateTime Timestamp { get; set; }  // When the message was sent
    }
}
