using IdentityCore.Models;
using System.Threading.Tasks;

namespace IdentityCore.Service
{
    public interface IEmailService
    {
        Task SendTestEmail(UserEmailOptions userEmailOptions);
        Task SendEmailConfirmation(UserEmailOptions userEmailOptions);
        Task SendEmailForgotPassword(UserEmailOptions userEmailOptions);
    }
}