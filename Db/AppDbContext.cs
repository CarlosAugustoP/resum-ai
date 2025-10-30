using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Resumai.Models;

namespace Resumai.Db
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Resume> Resumes { get; set; } = null!;
        public DbSet<JobExperience> JobExperiences { get; set; } = null!;
        public DbSet<Education> Educations { get; set; } = null!;
        public DbSet<Skill> Skills { get; set; } = null!;
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}