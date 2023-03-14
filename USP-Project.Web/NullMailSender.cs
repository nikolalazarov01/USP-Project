using Microsoft.AspNetCore.Identity.UI.Services;

namespace USP_Project.Web;

public class NullMailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
        => Task.CompletedTask;
}