using Bc = BCrypt.Net.BCrypt;

namespace Resumai.Auth
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return Bc.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return Bc.Verify(password, hash);
        }
    }
}