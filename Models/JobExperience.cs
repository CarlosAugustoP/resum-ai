using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resumai.Models
{
    public class JobExperience
    {
        public Guid Id { get; set; }
        public string Company  { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public User User { get; set; } = null!;
        public Guid UserId { get; set; }

        public JobExperience()
        {
        }

        public JobExperience(string role, string category, string title,
         string description, DateTime startDate, DateTime? endDate, Guid userID, string company)
        {
            Id = Guid.NewGuid();
            Role = role;
            Category = category;
            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            UserId = userID;
            Company = company;
        }
    }
}