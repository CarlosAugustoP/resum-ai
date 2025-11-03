using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Resumai.DTOs
{
    public record RegisterRequest(
        [Required]
        [MaxLength(50)]
        string Name,
        [Required]
        [MaxLength(30)]
        string UserName,
        [EmailAddress]
        string Email,
        [MaxLength(100)]
        string Location,
        [PasswordPropertyText]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        string Password
    );
}
    