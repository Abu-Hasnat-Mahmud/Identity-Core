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
    }
}