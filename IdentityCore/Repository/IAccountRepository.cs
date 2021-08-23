using IdentityCore.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IdentityCore.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUser(SignupUserModel userModel);
        Task<SignInResult> PasswordSignIn(SigninUserModel userModel);
        Task Signout();
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model);
        Task<IdentityResult> ConfirmEmail(string uId, string token);

        Task GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task GenerateEmailForgotPasswordTokenAsync(ApplicationUser user);

        Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model);
    }
}