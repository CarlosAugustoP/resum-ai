using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resumai.Models
{
    public enum ResumeStatus { Draft, Current, Inactive }
    public class Resume
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public ResumeStatus Status { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public string Title { get; set; } = null!;
        // will be jsonb in postgres
        public string Content { get; set; } = null!;

        public Resume()
        {
        }

        public Resume(Guid userId,  string content, string title, string summary)
        {
            Id = Guid.NewGuid();
            Status = ResumeStatus.Draft;
            CreatedAt = DateTime.UtcNow;
            UserId = userId;
            Content = content;
            Title = title;
            Summary = summary;
        }

        public void SetAsCurrent()
        {
            Status = ResumeStatus.Current;
        }
        public void SaveInactive()
        {
            Status = ResumeStatus.Inactive;
        }
        
    }
}