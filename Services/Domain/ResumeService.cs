using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Resumai.Abstractions;
using Resumai.Db;
using Resumai.DTOs;
using Resumai.Exceptions;
using Resumai.Models;
using Resumai.Services.Application;
using Resumai.Utils;

namespace Resumai.Services.Domain
{
    public class ResumeService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        public ResumeService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<UserResumeDTO> Create(UserDTO currentUser, string language)
        {
            if (!Language.IsValidLanguage(language))
            {
                language = Language.English;
            }
            var jobs = _db.JobExperiences
                .Where(x => x.UserId == currentUser.Id)
                .ToList()
                .Select(x => _mapper.Map<JobExperienceDTO>(x))
                 ?? [];
                
            var links = _db.Links
                .Where(x => x.UserId == currentUser.Id)
                .ToList()
                .Select(x => _mapper.Map<LinkDTO>(x))
                ?? [];

            var educations = _db.Educations
                .Where(x => x.UserId == currentUser.Id)
                .ToList()
                .Select(x => _mapper.Map<EducationDTO>(x))
                ?? [];

            var skills = _db.Skills
                .Where(x => x.UserId == currentUser.Id)
                .ToList()
                .Select(x => _mapper.Map<SkillDTO>(x))
                ?? [];

            var oldResume = _db.Resumes.FirstOrDefault(x => x.User.Id == currentUser.Id && x.Status == ResumeStatus.Current);

            if (oldResume != null)
                oldResume.Status = ResumeStatus.Inactive;

            var pb = new PromptBuilder()
                .WithUser(currentUser)
                .WithLanguage(language)
                .WithJobExperiences(jobs.ToList())
                .WithEducations(educations.ToList())
                .WithSkills(skills.ToList())
                .WithLinks(links.ToList());

            var response = await OpenAiService.Call(pb);
            var json = JsonSerializer.Serialize(response);

            _db.Add(new Resume(currentUser.Id, json, response.ResumeTitle ?? GetGenericTitle(currentUser), response.ResumeSummary));

            await _db.SaveChangesAsync();
            return response;
        }

        private static string GetGenericTitle(UserDTO u)
        {
            return $"{u.Name}'s Resume {DateTime.UtcNow:yyyy-MM-dd}";
        }

        public UserResumeDTO GetCurrent(UserDTO user)
        {
            var resume = _db.Resumes.FirstOrDefault(x => x.UserId == user.Id && x.Status == ResumeStatus.Current)
                ?? throw new NotFoundException("Current resume not found.");

            var actualResume = JsonSerializer.Deserialize<UserResumeDTO>(resume.Content)
                ?? throw new Exception("Failed to deserialize resume content.");

            return _mapper.Map<UserResumeDTO>(actualResume);
        }
        

        public bool SaveResume(UserDTO user, SaveResumeRequest req, Guid resumeId)
        {
            var resume = _db.Resumes.FirstOrDefault(x => x.Id == resumeId && x.UserId == user.Id)
                ?? throw new NotFoundException("Resume not found.");

            resume.Title = req.Title;
            resume.Summary = req.Summary;
            resume.SetAsCurrent();

            var oldResume = _db.Resumes.FirstOrDefault(x => x.UserId == user.Id && x.Status == ResumeStatus.Current && x.Id != resumeId);
            if (oldResume != null)
                oldResume.SaveInactive();

            _db.SaveChanges();
            return true;
        }

        public bool DeleteResume(UserDTO user, Guid resumeId)
        {
            var resume = _db.Resumes.FirstOrDefault(x => x.Id == resumeId && x.UserId == user.Id)
                ?? throw new NotFoundException("Resume not found.");

            _db.Resumes.Remove(resume);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateResume(UserDTO user, Guid resumeId, UserResumeDTO newResume)
        {
            var resume = _db.Resumes.FirstOrDefault(x => x.Id == resumeId && x.UserId == user.Id)
                ?? throw new NotFoundException("Resume not found.");

            resume.Content = JsonSerializer.Serialize(newResume);
            _db.SaveChanges();
            return true;
        }

        public UserResumeDTO UpdateResumeSummary(UserDTO user, Guid resumeId, SaveResumeRequest req)
        {
            var resume = _db.Resumes.FirstOrDefault(x => x.Id == resumeId && x.UserId == user.Id)
                ?? throw new NotFoundException("Resume not found.");

            resume.Summary = req.Summary;
            resume.Title = req.Title;
            _db.SaveChanges();
            return JsonSerializer.Deserialize<UserResumeDTO>(resume.Content)!;
        }

        public PaginatedList<UserResumeDTO> GetResumes(UserDTO user, PageRequest pageRequest)
        {
            var query = _db.Resumes
                .Where(x => x.UserId == user.Id)
                .OrderByDescending(x => x.CreatedAt);

            var paginatedResumes = query.Paginate(pageRequest);

            return paginatedResumes.Select(x => JsonSerializer.Deserialize<UserResumeDTO>(x.Content)!);
        }

        public PaginatedList<ResumeHeaderDTO> GetResumeHeaders(UserDTO user, PageRequest pageRequest)
        {
            var query = _db.Resumes
                .Where(x => x.UserId == user.Id)
                .OrderByDescending(x => x.CreatedAt);

            var paginatedResumes = query.Paginate(pageRequest);

            return paginatedResumes.Select(x =>
            {
                return new ResumeHeaderDTO(x.Title, x.Summary, x.Id);
            });
        }
    }
}