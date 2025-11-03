using AutoMapper;
namespace Resumai.DTOs
{
  public record CreateJobExperienceRequest 
  {
      public required string Title { get; init; }
      public required string Description { get; init; }
      public required DateTime StartDate { get; init; }
      public DateTime? EndDate { get; init; }
      public required string Role { get; init; }
      public required string Category { get; init; }
      public required string Company { get; init; }
  };
}