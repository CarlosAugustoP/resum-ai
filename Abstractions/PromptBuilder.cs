using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenAI.Chat;
using Resumai.DTOs;

namespace Resumai.Abstractions
{
    public class PromptBuilder
    {
        private UserDTO? _user;
        private List<JobExperienceDTO>? _jobs;
        private List<EducationDTO>? _educations;
        private List<SkillDTO>? _skills;
        private string? _textPrompt;
        private List<TagEnum>? _tags;
        public string? Language { get; set; }

        public PromptBuilder WithUser(UserDTO user)
        {
            _user = user;
            return this;
        }

        public PromptBuilder WithLanguage(string language)
        {
            Language = language;
            return this;
        }

        public PromptBuilder WithJobExperiences(List<JobExperienceDTO> jobs)
        {
            _jobs = jobs;
            return this;
        }

        public PromptBuilder WithEducations(List<EducationDTO> educations)
        {
            _educations = educations;
            return this;
        }

        public PromptBuilder WithSkills(List<SkillDTO> skills)
        {
            _skills = skills;
            return this;
        }

        public PromptBuilder WithTextPrompt(string textPrompt)
        {
            _textPrompt = textPrompt;
            return this;
        }
        public PromptBuilder WithTags(List<TagEnum> tags)
        {
            _tags = tags;
            return this;
        }

        public (string Prompt, ChatResponseFormat Format) Build()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Você é um assistente especializado em criar currículos profissionais e claros.");
            sb.AppendLine("Com base nas informações abaixo, gere um resumo estruturado conforme o formato JSON indicado.");

            if (_user is not null)
            {
                sb.AppendLine($"\nNome: {_user.Name}");
                sb.AppendLine($"Localização: {_user.Location}");
                sb.AppendLine($"Email: {_user.Email}");
            }

            if (_jobs is not null && _jobs.Any())
            {
                sb.AppendLine("\nExperiências profissionais:");
                foreach (var job in _jobs)
                {
                    sb.AppendLine($"- {job.Role} em {job.Company} ({job.StartDate:yyyy-MM} - {(job.EndDate?.ToString("yyyy-MM") ?? "atual")})");
                    sb.AppendLine($"  {job.Description}");
                }
            }

            if (_educations is not null && _educations.Any())
            {
                sb.AppendLine("\nFormação acadêmica:");
                foreach (var edu in _educations)
                {
                    sb.AppendLine($"- {edu.Degree} em {edu.Institution} ({edu.StartDate:yyyy-MM} - {(edu.EndDate?.ToString("yyyy-MM") ?? "atual")})");
                }
            }

            if (_skills is not null && _skills.Any())
            {
                sb.AppendLine("\nHabilidades:");
                foreach (var skill in _skills)
                {
                    sb.AppendLine($"- {skill.Name} (usada em: {skill.WhereUsed})");
                }
            }

            if (_tags is not null && _tags.Any())
            {
                sb.AppendLine("\nDeixe o currículo modelado às seguintes tags:");
                sb.AppendLine(string.Join(", ", _tags));
            }

            if (!string.IsNullOrWhiteSpace(_textPrompt))
            {
                sb.AppendLine("\nInstruções adicionais:");
                sb.AppendLine(_textPrompt);
            }

            if (!string.IsNullOrWhiteSpace(Language))
            {
                sb.AppendLine($"\nGere o currículo na língua {Language}.");
            }

            sb.AppendLine("\nRetorne a resposta no formato JSON definido a seguir.");

            var format = ChatResponseFormat.CreateJsonSchemaFormat(
                jsonSchemaFormatName: "resume_response",
                jsonSchema: BinaryData.FromString(@"
                {
                    ""type"": ""object"",
                    ""properties"": {
                        ""name"": { ""type"": ""string"" },
                        ""location"": { ""type"": ""string"" },
                        ""email"": { ""type"": ""string"" },
                        ""resume_title"": { ""type"": ""string"" },
                        ""resume_summary"": { ""type"": ""string"" },
                        ""key_skills"": { 
                            ""type"": ""array"", 
                            ""items"": { ""type"": ""string"" }
                        },
                        ""job_experiences"": {
                            ""type"": ""array"",
                            ""description"": ""Experiências de trabalho do usuário"",
                            ""items"": {
                                ""type"": ""object"",
                                ""properties"": {
                                    ""company"": { ""type"": ""string"" },
                                    ""title"": { ""type"": ""string"" },
                                    ""description"": { ""type"": ""string"" },
                                    ""start_date"": { ""type"": ""string"", ""format"": ""date"" },
                                    ""end_date"": { ""type"": [""string"", ""null""], ""format"": ""date"" },
                                    ""role"": { ""type"": ""string"" },
                                    ""category"": { ""type"": ""string"" }
                                },
                                ""required"": [""company"", ""title"", ""description"", ""start_date""]
                            }
                        },
                        ""educations"": {
                            ""type"": ""array"",
                            ""description"": ""Formações acadêmicas"",
                            ""items"": {
                                ""type"": ""object"",
                                ""properties"": {
                                    ""institution"": { ""type"": ""string"" },
                                    ""degree"": { ""type"": ""string"" },
                                    ""start_date"": { ""type"": ""string"", ""format"": ""date"" },
                                    ""end_date"": { ""type"": [""string"", ""null""], ""format"": ""date"" }
                                },
                                ""required"": [""institution"", ""degree"", ""start_date""]
                            }
                        },
                        ""skills"": {
                            ""type"": ""array"",
                            ""description"": ""Habilidades técnicas e interpessoais"",
                            ""items"": {
                                ""type"": ""object"",
                                ""properties"": {
                                    ""name"": { ""type"": ""string"" },
                                    ""where_used"": { ""type"": ""string"" }
                                },
                                ""required"": [""name"", ""where_used""]
                            }
                        }
                    },
                    ""required"": [""resume_summary"", ""key_skills"", ""job_experiences""]
                }")
            );

            return (sb.ToString(), format);
        }
    }
}
