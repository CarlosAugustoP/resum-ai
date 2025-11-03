using AutoMapper;

namespace Resumai.DTOs
{
    [AutoMap(typeof(Models.Link))]
    public class LinkDTO
    {
        public required Guid Id { get; set; }
        public required string Url { get; set; }
        public required string Description { get; set; }
        public required Guid UserId { get; set; }
    }
}