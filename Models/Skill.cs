namespace Resumai.Models
{
    public class Skill
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string WhereUsed { get; set; } = null!;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Skill()
        {
        }
        public Skill(string name, string whereUsed, Guid userId)
        {
            Id = Guid.NewGuid();
            Name = name;
            WhereUsed = whereUsed;
            UserId = userId;
        }
    }
}