namespace Resumai.Models
{
    public class Link
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Link()
        {
        }
        public Link(string url, Guid userId, string description)
        {
            Id = Guid.NewGuid();
            Url = url;
            UserId = userId;
            Description = description;
        }
    }
}