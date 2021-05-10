using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Word4Everyone.Services.Interfaces;

namespace Word4Everyone.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            string apiKey = _configuration["SendGrid:apiKey"];
            SendGridClient client = new SendGridClient(apiKey);
            EmailAddress from = new EmailAddress(_configuration["SendGrid:senderMail"], _configuration["SendGrid:senderName"]);
            EmailAddress to = new EmailAddress(toEmail);
            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}
