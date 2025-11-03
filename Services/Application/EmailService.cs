using MailKit.Net.Smtp;
using MimeKit;

namespace Application.Services {
    public class EmailService
    {
        private readonly string AuthenticationPassword;

        public EmailService(string authenticationPassword)
        {
            AuthenticationPassword = authenticationPassword;
        }

        public async Task SendEmail(string to, string body, string subject)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Resum.AI", "resumai@gmail.com"));
            email.To.Add(new MailboxAddress("", to));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("resumai@gmail.com", AuthenticationPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}