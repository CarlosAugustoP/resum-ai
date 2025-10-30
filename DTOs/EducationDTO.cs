using AutoMapper;
using Resumai.Models;

namespace Resumai.DTOs
{
    [AutoMap(typeof(Education))]
    public class EducationDTO
    {
        public required Guid Id { get; set; }
        public required string Institution { get; set; }
        public required string Degree { get; set; }
        public required DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public required Guid UserID { get; set; }
    }
}