using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Resumai.DTOs
{
    public class UserResumeDTO
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("resume_title")]
        public string? ResumeTitle { get; set; }

        [JsonPropertyName("resume_summary")]
        public string ResumeSummary { get; set; } = string.Empty;

        [JsonPropertyName("key_skills")]
        public List<string> KeySkills { get; set; } = new();

        [JsonPropertyName("job_experiences")]
        public List<JobExperienceItemDTO> JobExperiences { get; set; } = new();

        [JsonPropertyName("educations")]
        public List<EducationItemDTO>? Educations { get; set; }

        [JsonPropertyName("skills")]
        public List<SkillItemDTO>? Skills { get; set; }

        [JsonPropertyName("links")]
        public Dictionary<string, string>? Links { get; set; }
    }

    public class JobExperienceItemDTO
    {
        [JsonPropertyName("company")]
        public string Company { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("role")]
        public string? Role { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }
    }

    public class EducationItemDTO
    {
        [JsonPropertyName("institution")]
        public string Institution { get; set; } = string.Empty;

        [JsonPropertyName("degree")]
        public string Degree { get; set; } = string.Empty;

        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime? EndDate { get; set; }
    }

    public class SkillItemDTO
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("where_used")]
        public string WhereUsed { get; set; } = string.Empty;
    }
}
