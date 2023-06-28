namespace Petrof.Utility
{
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Configuration;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System.Threading.Tasks;
    public class EmailSender : IEmailSender
    {
        //public string SendGridSecret { get; set; }

        //public EmailSender(IConfiguration _config)
        //{
        //    _config.GetValue<string>("SendGrid:SecretKey");
        //}
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //var client = new SendGridClient(SendGridSecret);
            //var from = new EmailAddress("company@mail.address", "Bulky Book");
            //var to = new EmailAddress(email);
            //var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            //return client.SendEmailAsync(message);
            return Task.CompletedTask;
        }
    }
}