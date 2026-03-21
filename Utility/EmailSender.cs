using System;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace LoanApp.Utility
{

    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //real email sender logic here later
            return Task.CompletedTask;
        }
    }

}
