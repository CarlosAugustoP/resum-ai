using Microsoft.AspNetCore.Mvc;
using Resumai.DTOs;
using Resumai.Services;
using Resumai.Middlewares;
using Resumai.Services.Domain;
using Resumai.Utils;
using Resumai.Abstractions;

namespace Resumai.Controllers
{
    [ApiController]
    [Route("api/links")]
    public class LinksController : ResumaiController
    {
        private readonly UserService _linkService;

        public LinksController(UserService linkService)
        {
            _linkService = linkService;
        }

        [HttpGet]
        [RequireProfileFilter]
        public ActionResult<PaginatedList<LinkDTO>> GetLinks([FromQuery] PageRequest request)
        {
            var links = _linkService.GetLinks(CurrentUser!, request);
            return Ok(Result<PaginatedList<LinkDTO>>.Success(links));
        }

        [HttpGet("{id:guid}")]
        [RequireProfileFilter]
        public ActionResult<LinkDTO> GetLinkById(Guid id)
        {
            var link = _linkService.GetLinkById(CurrentUser!, id);
            return Ok(Result<LinkDTO>.Success(link));
        }

        [HttpPost]
        [RequireProfileFilter]
        public ActionResult<LinkDTO> AddLink([FromBody] CreateLinkRequest request)
        {
            var link = _linkService.AddLink(CurrentUser!, request.Url, request.Description);
            return Ok(Result<LinkDTO>.Success(link));
        }

        [HttpPut("{id:guid}")]
        public ActionResult<LinkDTO> UpdateLink(Guid id, [FromBody] UpdateLinkRequest request)
        {
            var link = _linkService.UpdateLink(CurrentUser, id, request.Url, request.Description);
            return Ok(Result<LinkDTO>.Success(link));
        }

        // DELETE api/links/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteLink(Guid id)
        {
            _linkService.DeleteLink(CurrentUser!, id);
            return NoContent();
        }
    }

    // DTOs auxiliares
    public class CreateLinkRequest
    {
        public string Url { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class UpdateLinkRequest
    {
        public string Url { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
