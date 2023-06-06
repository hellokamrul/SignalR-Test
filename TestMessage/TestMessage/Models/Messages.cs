namespace TestMessage.Models
{
    public class Messages
    {
            public int Id { get; set; }
            public string Content { get; set; }
            public DateTime SentAt { get; set; }
            public User Sender { get; set; }
            public Groups Group { get; set; }
    }
}
