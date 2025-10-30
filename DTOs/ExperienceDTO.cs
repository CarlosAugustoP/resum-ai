using Resumai.Models;
using AutoMapper;
namespace Resumai.DTOs
{
    [AutoMap(typeof(JobExperience))]
    public class JobExperienceDTO
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public required Guid UserID { get; set; }
        public required string Role { get; set; }
        public required string Category { get; set; }
        public required string Company { get; set; } 
    }
}