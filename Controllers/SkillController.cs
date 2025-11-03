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
    [Route("api/skills")]
    public class SkillController : ResumaiController
    {
        private readonly UserService _userService;

        public SkillController(UserService userService)
        {
            _userService = userService;
        }

        [RequireProfileFilter]
        [HttpGet]
        public IActionResult GetSkills([FromQuery] PageRequest pageRequest)
        {
            var skills = _userService.GetSkills(CurrentUser!, pageRequest);
            return Ok(Result<PaginatedList<SkillDTO>>.Success(skills));
        }

        [RequireProfileFilter]
        [HttpGet("{id:guid}")]
        public IActionResult GetSkillById(Guid id)
        {
            var skill = _userService.GetSkillById(CurrentUser!, id);
            return Ok(Result<SkillDTO>.Success(skill));
        }

        [RequireProfileFilter]
        [HttpPost]
        public IActionResult AddSkill([FromBody] SkillCreateRequest request)
        {
            var newSkill = _userService.AddSkill(CurrentUser!, request.Name, request.WhereUsed);
            return Ok(Result<SkillDTO>.Success(newSkill));
        }

        [RequireProfileFilter]
        [HttpPut("{id:guid}")]
        public IActionResult UpdateSkill(Guid id, [FromBody] SkillCreateRequest request)
        {
            var updatedSkill = _userService.UpdateSkill(CurrentUser!, id, request.Name, request.WhereUsed);
            return Ok(Result<SkillDTO>.Success(updatedSkill));
        }

        [RequireProfileFilter]
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteSkill(Guid id)
        {
            _userService.DeleteSkill(CurrentUser!, id);
            return NoContent();
        }
    }
}
