using Microsoft.AspNetCore.Mvc;
using Resumai.Abstractions;
using Resumai.DTOs;
using Resumai.DTOs.Requests;
using Resumai.Middlewares;
using Resumai.Services.Domain;
using Resumai.Utils;

namespace Resumai.Controllers
{
    [ApiController]
    [Route("api/job-experiences")]
    public class JobExperienceController : ResumaiController
    {
        private readonly UserService _userService;

        public JobExperienceController(UserService userService)
        {
            _userService = userService;
        }

        [RequireProfileFilter]
        [HttpGet]
        public IActionResult GetJobExperiences([FromQuery] PageRequest pageRequest)
        {
            var jobExperiences = _userService.GetJobExperiences(CurrentUser!, pageRequest);
            return Ok(Result<PaginatedList<JobExperienceDTO>>.Success(jobExperiences));
        }

        [RequireProfileFilter]
        [HttpGet("{id:guid}")]
        public IActionResult GetJobExperienceById(Guid id)
        {
            var jobExperience = _userService.GetJobExperienceById(CurrentUser!, id);
            return Ok(Result<JobExperienceDTO>.Success(jobExperience));
        }

        [RequireProfileFilter]
        [HttpPost]
        public IActionResult CreateJobExperience([FromBody] CreateJobExperienceRequest request)
        {
            var newJob = _userService.AddJobExperience(CurrentUser!, request);
            return Ok(Result<JobExperienceDTO>.Success(newJob));
        }

        [RequireProfileFilter]
        [HttpPut("{id:guid}")]
        public IActionResult UpdateJobExperience(Guid id, [FromBody] CreateJobExperienceRequest request)
        {
            var updatedJob = _userService.UpdateJobExperience(CurrentUser!, id, request);
            return Ok(Result<JobExperienceDTO>.Success(updatedJob));
        }

        [RequireProfileFilter]
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteJobExperience(Guid id)
        {
            _userService.DeleteJobExperice(CurrentUser!, id);
            return NoContent();
        }
    }
}
