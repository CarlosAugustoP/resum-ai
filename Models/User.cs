using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resumai.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string JoinedAt { get; set; } = null!;
        public List<Resume> Resumes { get; set; } = new();

        public User()
        {
        }
        public User(string username, string email, string passwordHash, string joinedAt)
        {
            Id = Guid.NewGuid();
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            JoinedAt = joinedAt;
        }
    }
}