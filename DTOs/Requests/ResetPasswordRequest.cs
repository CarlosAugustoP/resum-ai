namespace Resumai.DTOs.Requests
{
    public record ResetPasswordRequest(string OTP, string NewPassword);
}