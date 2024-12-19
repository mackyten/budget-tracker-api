using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BT.SERVICES.EmailSender
{
    public class NoOpEmailSender<TUser> : IEmailSender<TUser> where TUser : IdentityUser

    {
        public Task SendConfirmationLinkAsync(TUser user, string email, string confirmationLink)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // No-op implementation
            return Task.CompletedTask;
        }

        public Task SendPasswordResetCodeAsync(TUser user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetLinkAsync(TUser user, string email, string resetLink)
        {
            throw new NotImplementedException();
        }
    }
}