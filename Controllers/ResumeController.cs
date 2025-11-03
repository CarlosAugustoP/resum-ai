using Microsoft.AspNetCore.Mvc;
using Resumai.Abstractions;
using Resumai.DTOs;
using Resumai.Middlewares;
using Resumai.Services.Domain;
using Resumai.Utils;

namespace Resumai.Controllers
{
    [ApiController]
    [Route("api/resumes")]
    public class ResumeController : ResumaiController
    {
        private readonly ResumeService _resumeService;

        public ResumeController(ResumeService resumeService)
        {
            _resumeService = resumeService;
        }

        [RequireProfileFilter]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromQuery] string language = "en")
        {
            var resume = await _resumeService.Create(CurrentUser!, language);
            return Ok(Result<UserResumeDTO>.Success(resume));
        }

        [RequireProfileFilter]
        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            var resume = _resumeService.GetCurrent(CurrentUser!);
            return Ok(Result<UserResumeDTO>.Success(resume));
        }

        [RequireProfileFilter]
        [HttpGet]
        public IActionResult GetAll([FromQuery] PageRequest page)
        {
            var resumes = _resumeService.GetResumes(CurrentUser!, page);
            return Ok(Result<PaginatedList<UserResumeDTO>>.Success(resumes));
        }

        [RequireProfileFilter]
        [HttpGet("headers")]
        public IActionResult GetHeaders([FromQuery] PageRequest page)
        {
            var headers = _resumeService.GetResumeHeaders(CurrentUser!, page);
            return Ok(Result<PaginatedList<ResumeHeaderDTO>>.Success(headers));
        }

        [RequireProfileFilter]
        [HttpPost("{resumeId:guid}/save")]
        public IActionResult Save(Guid resumeId, [FromBody] SaveResumeRequest request)
        {
            var result = _resumeService.SaveResume(CurrentUser!, request, resumeId);
            return Ok(Result<bool>.Success(result));
        }

        [RequireProfileFilter]
        [HttpDelete("{resumeId:guid}")]
        public IActionResult Delete(Guid resumeId)
        {
            var result = _resumeService.DeleteResume(CurrentUser!, resumeId);
            return NoContent();
        }

        [RequireProfileFilter]
        [HttpPut("{resumeId:guid}")]
        public IActionResult Update(Guid resumeId, [FromBody] UserResumeDTO newResume)
        {
            var result = _resumeService.UpdateResume(CurrentUser!, resumeId, newResume);
            return Ok(Result<bool>.Success(result));
        }

        [RequireProfileFilter]
        [HttpPatch("{resumeId:guid}/summary")]
        public IActionResult UpdateSummary(Guid resumeId, [FromBody] SaveResumeRequest request)
        {
            var updated = _resumeService.UpdateResumeSummary(CurrentUser!, resumeId, request);
            return Ok(Result<UserResumeDTO>.Success(updated));
        }
    }
}
