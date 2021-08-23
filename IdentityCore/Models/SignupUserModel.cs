using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityCore.Models
{
    public class SignupUserModel
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Birth Date")]
        public DateTime? DateOfBirth { get; set; }



        [DisplayName("User Email")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Password not match")]
        [Required(ErrorMessage = "Enter your  password")]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter confirm password")]
        public string ConfirmPassword { get; set; }
    }

    public class SigninUserModel
    {
        [DisplayName("Email")]
        [Required]
        public string Email { get; set; }

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }

    }



    public class ChangePasswordModel
    {


        [DisplayName("Crrent Password")]
        [DataType(DataType.Password)]
        [Required]
        public string CurrentPassword { get; set; }

        [DisplayName("New Password")]
        [DataType(DataType.Password)]
        [Required]
        public string NewPassword { get; set; }

        [DisplayName("Confirm Pasword")]
        [DataType(DataType.Password)]
        [Required]
        [Compare("NewPassword", ErrorMessage = "Password not match")]
        public string ConfirmPassword { get; set; }




    }

    public class EmailConfrimModel
    {
        public string Email { get; set; }
        //public bool IsConfirmed { get; set; }
        public bool EmailSent { get; set; }
        public bool EmailVerified { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required, EmailAddress, Display(Name = "Registered email address")]
        public string Email { get; set; }
        public bool EmailSent { get; set; }
    }

    public class ResetPasswordModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
        public bool IsSuccess { get; set; }

    }



}
