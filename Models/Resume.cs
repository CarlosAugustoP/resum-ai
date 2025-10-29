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
        public string Title { get; set; } = null!;
        public string CreatedAt { get; set; } = null!;
        public ResumeStatus Status { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // will be jsonb in postgres
        public string Content { get; set; } = null!;

        public Resume()
        {
        }

        public Resume(string title, string createdAt, ResumeStatus status, Guid userId, string content)
        {
            Id = Guid.NewGuid();
            Title = title;
            CreatedAt = createdAt;
            Status = status;
            UserId = userId;
            Content = content;
        }
    }
}