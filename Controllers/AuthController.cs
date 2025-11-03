using Microsoft.AspNetCore.Mvc;
using Resumai.Abstractions;
using Resumai.DTOs;
using Resumai.Middlewares;
using Resumai.Services.Domain;
using Requests = Resumai.DTOs.Requests;
namespace Resumai.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ResumaiController
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [RequireProfileFilter]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            return Ok(Result<UserDTO>.Success(CurrentUser!));
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Requests.LoginRequest request)
        {
            var token = _userService.Login(request.Email, request.Password);
            return Ok(Result<string>.Success(token));
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            var success = _userService.Register(
                request.UserName,
                request.Name,
                request.Location,
                request.Email,
                request.Password
            );

            return Ok(Result<bool>.Success(success));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] UserDTO user)
        {
            var success = await _userService.ForgotPasswordRequest(user);
            return Ok(Result<bool>.Success(success));
        }

        [RequireProfileFilter]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] Requests.ResetPasswordRequest request)
        {
            var success = await _userService.ResetPassword(CurrentUser!, request.OTP, request.NewPassword);
            return Ok(Result<bool>.Success(success));
        }
    }
}
