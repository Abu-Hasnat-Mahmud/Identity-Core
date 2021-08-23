using IdentityCore.Models;
using IdentityCore.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityCore.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly Service.IUserService _userService;
        private IConfiguration _configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, Service.IUserService userService, IEmailService emailService, IConfiguration configuration)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _configuration = configuration;
        }


        public async Task<IdentityResult> CreateUser(SignupUserModel userModel)
        {
            var user = new ApplicationUser
            {
                Email = userModel.Email,
                UserName = userModel.Email,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                DateOfBirth = userModel.DateOfBirth,
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            if (result.Succeeded)
            {
                await GenerateEmailConfirmationTokenAsync(user);
            }

            return result;
        }

        public async Task GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (!String.IsNullOrEmpty(token))
            {
                await SendEmailforEmailConfirmation(user, token);
            }
        }

        public async Task GenerateEmailForgotPasswordTokenAsync(ApplicationUser user)
        {
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (!String.IsNullOrEmpty(token))
            {
                await SendEmailforForgotPassword(user, token);
            }
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }



        public async Task<SignInResult> PasswordSignIn(SigninUserModel userModel)
        {

            var result = await _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password, userModel.RememberMe, false);

            return result;
        }

        public async Task Signout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model)
        {
            var userId = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model)
        {          
            return await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(model.UserId), model.Token, model.NewPassword);
        }

        public async Task<IdentityResult> ConfirmEmail(string uId, string token)
        {
            return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uId), token);
        }


        private async Task SendEmailforEmailConfirmation(ApplicationUser user, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmaitonLink = _configuration.GetSection("Application:EmailConfirmation").Value;

            UserEmailOptions userEmailOptions = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{username}}",user.FirstName),
                    new KeyValuePair<string, string>("{{link}}",String.Format(appDomain+confirmaitonLink,user.Id,token))
                }
            };

            await _emailService.SendEmailConfirmation(userEmailOptions);
        }

        private async Task SendEmailforForgotPassword(ApplicationUser user, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmaitonLink = _configuration.GetSection("Application:ForgotPassword").Value;

            UserEmailOptions userEmailOptions = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{username}}",user.FirstName),
                    new KeyValuePair<string, string>("{{link}}",String.Format(appDomain+confirmaitonLink,user.Id,token))
                }
            };

            await _emailService.SendEmailForgotPassword(userEmailOptions);
        }

    }
}
