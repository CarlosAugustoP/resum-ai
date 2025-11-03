namespace Resumai.DTOs.Requests
{
    public record CreateEducationRequest(
        string Institution,
        string Degree,
        DateTime StartDate,
        DateTime? EndDate
    );
}