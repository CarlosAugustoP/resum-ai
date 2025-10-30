namespace Resumai.Models
{
    public class Education
    {
        public Guid Id { get; set; }
        public string Institution { get; set; } = null!;
        public string Degree { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid UserId { get; set; }

        public Education()
        {
        }

        public Education(string institution, string degree,
         DateTime startDate, DateTime? endDate, Guid userID)
        {
            Id = Guid.NewGuid();
            Institution = institution;
            Degree = degree;
            StartDate = startDate;
            EndDate = endDate;
            UserId = userID;
        }
    }
}