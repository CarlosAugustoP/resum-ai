using AutoMapper;

namespace Resumai.DTOs
{
    [AutoMap(typeof(Models.Skill))]
    public class SkillDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string WhereUsed { get; set; }
        public required Guid UserId { get; set; }
    }
}