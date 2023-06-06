namespace TestMessage.Models
{
    public class Groups
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }

}
