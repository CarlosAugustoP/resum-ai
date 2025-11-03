using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Resumai.Auth;
using Resumai.Db;
using Resumai.DTOs;
using Resumai.DTOs.Requests;
using Resumai.Exceptions;
using Resumai.Utils;
using ZiggyCreatures.Caching.Fusion;

namespace Resumai.Services.Domain
{
    public class UserService
    {
        private readonly AppDbContext _db;
        private readonly EmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IFusionCache _cache;
        private readonly JwtService _jwtService;
        public UserService(AppDbContext db, EmailService emailService, JwtService jwtService, IFusionCache cache, IMapper mapper)
        {
            _db = db;
            _emailService = emailService;
            _jwtService = jwtService;
            _cache = cache;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public string Login(string email, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == email)
                ?? throw new UnauthorizedAccessException("Invalid email or password.");

            if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
            {
                return _jwtService.GenerateToken(user);
            }

            else throw new UnauthorizedAccessException();
        }

        public bool Register(string username, string name, string location, string email, string password)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser != null)
            {
                throw new ConflictException("Email is already registered.");
            }

            var passwordHash = PasswordHasher.HashPassword(password);
            var joinedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            var newUser = new Models.User(username, name, location, email, passwordHash, joinedAt);
            _db.Users.Add(newUser);
            _db.SaveChanges();
            return true;
        }

        public async Task<bool> ForgotPasswordRequest(UserDTO currentUser)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == currentUser.Id);

            if (user == null) return true;

            var OTP = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            await _emailService.SendEmail(
                to: user.Email,
                subject: "Password Reset Request",
                body: $"Your OTP for password reset is: {OTP}"
            );

            _cache.Set("pwd-reset-" + user.Id.ToString(), OTP, TimeSpan.FromMinutes(15));

            return true;
        }


        public async Task<bool> ResetPassword(UserDTO currentUser, string otp, string newPassword)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == currentUser.Id);

            if (user == null) return true;

            var cachedOtp = _cache.GetOrDefault<string>("pwd-reset-" + user.Id.ToString());

            if (cachedOtp == null || cachedOtp != otp)
            {
                throw new UnauthorizedAccessException("Invalid or expired code. Please, request a new code.");
            }

            user.PasswordHash = PasswordHasher.HashPassword(newPassword);
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            _cache.Remove("pwd-reset-" + user.Id.ToString());

            return true;
        }

        #region Job Experiences CRUD
        public PaginatedList<JobExperienceDTO> GetJobExperiences(UserDTO currentUser, PageRequest pr)
        {
            return _db.JobExperiences
                .Where(x => x.UserId == currentUser.Id)
                .Paginate(pr)
                .Select(x => _mapper.Map<JobExperienceDTO>(x));
        }

        public JobExperienceDTO GetJobExperienceById(UserDTO currentUser, Guid jobId)
        {
            var jobExperience = _db.JobExperiences.FirstOrDefault(x => x.Id == jobId && x.UserId == currentUser.Id);

            if (jobExperience == null)
                throw new NotFoundException("Job experience not found.");

            return _mapper.Map<JobExperienceDTO>(jobExperience);
        }

        public void DeleteJobExperice(UserDTO currentUser, Guid jobId)
        {
            var jobExperience = _db.JobExperiences.FirstOrDefault(x => x.Id == jobId);

            if (jobExperience == null || jobExperience.UserId != currentUser.Id)
                throw new NotFoundException("Job experience not found.");

            _db.JobExperiences.Remove(jobExperience);
            _db.SaveChanges();
        }

        public JobExperienceDTO AddJobExperience(UserDTO user, CreateJobExperienceRequest req)
        {
            var jobExperience = new Models.JobExperience
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Role = req.Role,
                Category = req.Category,
                Company = req.Company,
                StartDate = req.StartDate,
                EndDate = req.EndDate,
                Description = req.Description,
            };

            _db.JobExperiences.Add(jobExperience);
            _db.SaveChanges();

            return _mapper.Map<JobExperienceDTO>(jobExperience);
        }

        public JobExperienceDTO UpdateJobExperience(UserDTO user, Guid jobId, CreateJobExperienceRequest req)
        {
            var jobExperience = _db.JobExperiences.FirstOrDefault(x => x.Id == jobId);

            if (jobExperience == null || jobExperience.UserId != user.Id)
                throw new NotFoundException("Job experience not found.");

            jobExperience.Role = req.Role;
            jobExperience.Category = req.Category;
            jobExperience.Company = req.Company;
            jobExperience.StartDate = req.StartDate;
            jobExperience.EndDate = req.EndDate;
            jobExperience.Description = req.Description;

            _db.JobExperiences.Update(jobExperience);
            _db.SaveChanges();

            return _mapper.Map<JobExperienceDTO>(jobExperience);
        }

        #endregion
        #region Education CRUD
        public EducationDTO AddEducation(UserDTO user, CreateEducationRequest req)
        {
            var education = new Models.Education
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Institution = req.Institution,
                Degree = req.Degree,
                StartDate = req.StartDate,
                EndDate = req.EndDate,
            };

            _db.Educations.Add(education);
            _db.SaveChanges();

            return _mapper.Map<EducationDTO>(education);
        }
        public PaginatedList<EducationDTO> GetEducations(UserDTO currentUser, PageRequest pr)
        {
            return _db.Educations
                .Where(x => x.UserId == currentUser.Id)
                .Paginate(pr)
                .Select(x => _mapper.Map<EducationDTO>(x));
        }

        public void DeleteEducation(UserDTO currentUser, Guid educationId)
        {
            var education = _db.Educations.FirstOrDefault(x => x.Id == educationId);

            if (education == null || education.UserId != currentUser.Id)
                throw new NotFoundException("Education not found.");

            _db.Educations.Remove(education);
            _db.SaveChanges();
        }

        public EducationDTO UpdateEducation(UserDTO user, Guid educationId, CreateEducationRequest req)
        {
            var education = _db.Educations.FirstOrDefault(x => x.Id == educationId);

            if (education == null || education.UserId != user.Id)
                throw new NotFoundException("Education not found.");

            education.Institution = req.Institution;
            education.Degree = req.Degree;
            education.StartDate = req.StartDate;
            education.EndDate = req.EndDate;

            _db.Educations.Update(education);
            _db.SaveChanges();

            return _mapper.Map<EducationDTO>(education);
        }

        public EducationDTO GetEducationById(UserDTO currentUser, Guid educationId)
        {
            var education = _db.Educations.FirstOrDefault(x => x.Id == educationId && x.UserId == currentUser.Id);

            if (education == null)
                throw new NotFoundException("Education not found.");

            return _mapper.Map<EducationDTO>(education);
        }

        #endregion
        #region Skills CRUD

        public SkillDTO AddSkill(UserDTO user, string Name, string WhereUsed)
        {
            if (!_db.JobExperiences.Any(x => x.Company == WhereUsed && x.UserId == user.Id) &&
                !_db.Educations.Any(x => x.Institution == WhereUsed && x.UserId == user.Id))
            {
                throw new NotFoundException("The specified job does not match any of your job experiences or educations.");
            }

            var skill = new Models.Skill(Name, WhereUsed, user.Id);
            _db.Skills.Add(skill);
            _db.SaveChanges();

            return _mapper.Map<SkillDTO>(skill);
        }

        public PaginatedList<SkillDTO> GetSkills(UserDTO currentUser, PageRequest pr)
        {
            return _db.Skills
                .Where(x => x.UserId == currentUser.Id)
                .Paginate(pr)
                .Select(x => _mapper.Map<SkillDTO>(x));
        }

        public void DeleteSkill(UserDTO currentUser, Guid skillId)
        {
            var skill = _db.Skills.FirstOrDefault(x => x.Id == skillId);

            if (skill == null || skill.UserId != currentUser.Id)
                throw new NotFoundException("Skill not found.");

            _db.Skills.Remove(skill);
            _db.SaveChanges();
        }

        public SkillDTO UpdateSkill(UserDTO user, Guid skillId, string Name, string WhereUsed)
        {
            var skill = _db.Skills.FirstOrDefault(x => x.Id == skillId);

            if (skill == null || skill.UserId != user.Id)
                throw new NotFoundException("Skill not found.");

            if (!_db.JobExperiences.Any(x => x.Company == WhereUsed && x.UserId == user.Id) &&
                !_db.Educations.Any(x => x.Institution == WhereUsed && x.UserId == user.Id))
            {
                throw new NotFoundException("The specified job does not match any of your job experiences or educations.");
            }

            skill.Name = Name;
            skill.WhereUsed = WhereUsed;

            _db.Skills.Update(skill);
            _db.SaveChanges();

            return _mapper.Map<SkillDTO>(skill);
        }

        public SkillDTO GetSkillById(UserDTO currentUser, Guid skillId)
        {
            var skill = _db.Skills.FirstOrDefault(x => x.Id == skillId && x.UserId == currentUser.Id);

            if (skill == null)
                throw new NotFoundException("Skill not found.");

            return _mapper.Map<SkillDTO>(skill);
        }


        #endregion
        #region Links CRUD
        public LinkDTO AddLink(UserDTO user, string url, string description)
        {
            var link = new Models.Link(url, user.Id, description);
            _db.Links.Add(link);
            _db.SaveChanges();

            return _mapper.Map<LinkDTO>(link);
        }

        public PaginatedList<LinkDTO> GetLinks(UserDTO currentUser, PageRequest pr)
        {
            return _db.Links
                .Where(x => x.UserId == currentUser.Id)
                .Paginate(pr)
                .Select(x => _mapper.Map<LinkDTO>(x));
        }

        public void DeleteLink(UserDTO currentUser, Guid linkId)
        {
            var link = _db.Links.FirstOrDefault(x => x.Id == linkId);

            if (link == null || link.UserId != currentUser.Id)
                throw new NotFoundException("Link not found.");

            _db.Links.Remove(link);
            _db.SaveChanges();
        }

        public LinkDTO UpdateLink(UserDTO user, Guid linkId, string url, string description)
        {
            var link = _db.Links.FirstOrDefault(x => x.Id == linkId);

            if (link == null || link.UserId != user.Id)
                throw new NotFoundException("Link not found.");

            link.Url = url;
            link.Description = description;

            _db.Links.Update(link);
            _db.SaveChanges();

            return _mapper.Map<LinkDTO>(link);
        }

        public LinkDTO GetLinkById(UserDTO currentUser, Guid linkId)
        {
            var link = _db.Links.FirstOrDefault(x => x.Id == linkId && x.UserId == currentUser.Id);

            if (link == null)
                throw new NotFoundException("Link not found.");

            return _mapper.Map<LinkDTO>(link);
        }
    }
    #endregion
}