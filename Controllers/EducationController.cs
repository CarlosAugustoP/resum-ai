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
    [Route("api/educations")]
    public class EducationController : ResumaiController
    {
        private readonly UserService _userService;

        public EducationController(UserService userService)
        {
            _userService = userService;
        }

        [RequireProfileFilter]
        [HttpGet]
        public IActionResult GetEducations([FromQuery] PageRequest pageRequest)
        {
            var educations = _userService.GetEducations(CurrentUser!, pageRequest);
            return Ok(Result<PaginatedList<EducationDTO>>.Success(educations));
        }

        [RequireProfileFilter]
        [HttpGet("{id:guid}")]
        public IActionResult GetEducationById(Guid id)
        {
            var education = _userService.GetEducationById(CurrentUser!, id);
            return Ok(Result<EducationDTO>.Success(education));
        }
        [RequireProfileFilter]
        [HttpPost]
        public IActionResult AddEducation([FromBody] CreateEducationRequest request)
        {
            var newEducation = _userService.AddEducation(CurrentUser!, request);
            return Ok(Result<EducationDTO>.Success(newEducation));
        }

        [RequireProfileFilter]
        [HttpPut("{id:guid}")]
        public IActionResult UpdateEducation(Guid id, [FromBody] CreateEducationRequest request)
        {
            var updatedEducation = _userService.UpdateEducation(CurrentUser!, id, request);
            return Ok(Result<EducationDTO>.Success(updatedEducation));
        }

        [RequireProfileFilter]
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteEducation(Guid id)
        {
            _userService.DeleteEducation(CurrentUser!, id);
            return NoContent();
        }
    }
}
