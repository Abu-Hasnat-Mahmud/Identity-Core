using IdentityCore.Models;
using IdentityCore.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityCore.Controllers
{
    public class AccountController : Controller
    {

        private readonly IAccountRepository _accountRepository;
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;

        }
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupUserModel user)
        {
            if (ModelState.IsValid)
            {
                var result =await _accountRepository.CreateUser(user);

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("",item.Description);
                    }
                    return View();
                }
                ModelState.Clear();
            }
            return View();
        }


        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SigninUserModel user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result =await _accountRepository.PasswordSignIn(user);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("Index","Home");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "User not allowed");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credential");
                }
               
            }
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            await _accountRepository.Signout();
            return RedirectToAction("Index","Home");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.ChangePasswordAsync(model);

                if (result.Succeeded)
                {
                    ModelState.Clear();
                    ViewBag.IsSuccess = true;
                    return View();
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }           
            return View(model);
        }

        [HttpGet("Confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uId, string token)
        {
            if (!string.IsNullOrEmpty(uId) && !String.IsNullOrEmpty(token))
            {
                token = token.Replace(" ", "+");
                 var result=await _accountRepository.ConfirmEmail(uId, token);
                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                }
            }
            return View();
        }

    }
}
