using CarDealer.AppIdentity;
using CarDealer.DataAcces.Abstract;
using CarDealer.ExtensionMethods;
using CarDealer.Models;
using CarDealer.Provider;
using CarDealer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarDealer.Controllers
{
    public class AccountController : Controller
    {

        private UserManager<AppIdentityUser> userManager;
        private SignInManager<AppIdentityUser> signInManager;
        private RoleManager<AppIdentityRole> roleManager;
        private IActivityLogDal activityLogDal;
        private ISessionService SessionService;
        private ISmtpService smtpService;

        public AccountController(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager, RoleManager<AppIdentityRole> roleManager, ISessionService SessionService, ISmtpService smtpService, IActivityLogDal activityLogDal)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.SessionService = SessionService;
            this.smtpService = smtpService;
            this.activityLogDal = activityLogDal;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel userLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(userLoginViewModel.UserName);
                if (user != null)
                {

                    if (!await userManager.IsEmailConfirmedAsync(user))
                    {
                        TempData.Add("VerificationError", "Confirm your email please");
                        return View(userLoginViewModel);
                    }
                    var result = await signInManager.PasswordSignInAsync(user, userLoginViewModel.Password, userLoginViewModel.RememberMe, true);
                    if (result.RequiresTwoFactor)
                    {
                        SessionService.SetValue(user.UserName);
                        return RedirectToAction("TwoFactorCode");
                    }
                    if (result.IsLockedOut)
                    {
                        TempData.Add("Lockout", "Your account is locked. Due to unsuccessful password attempts.Please relogin later.");
                    }
                    if (result.Succeeded)
                    {
                        SessionService.SetValue(user.UserName);
                        return RedirectToAction("HomePage", "Management");
                    }

                }
                TempData.Add("LoginUserNotFound", "Your password or username is invlaid");
            }
            return View(userLoginViewModel);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var userCheck = userManager.FindByNameAsync(registerViewModel.Username);
                if (userCheck == null)
                {
                    TempData.Add("UserExist", "This username was taken chose diffrent username");
                    return View(registerViewModel);
                }
                if (registerViewModel.Password != registerViewModel.ConfirmPassword)
                {
                    TempData.Add("PasswordsNotMatch", "Your must enter same password value");
                    return View(registerViewModel);
                }
                AppIdentityUser user = new AppIdentityUser
                {
                    UserName = registerViewModel.Username,
                    Email = registerViewModel.Email
                };
                var result = await userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync("Manager"))
                    {
                        AppIdentityRole appIdentityRole = new AppIdentityRole
                        {
                            Name = "Manager"
                        };
                        var roleResult = await roleManager.CreateAsync(appIdentityRole);
                        if (!roleResult.Succeeded)
                        {
                            TempData.Add("RoleMessage", String.Format("We can't add the role"));
                            return View(registerViewModel);
                        }
                    }
                    userManager.AddToRoleAsync(user, "Manager").Wait();
                    var emailConfirmCode = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var ConfirmUrl = Url.Action("ConfirmMail", "Account", new { userId = user.Id, Code = emailConfirmCode });
                    smtpService.AccountConfirmMail(ConfirmUrl, user.UserName, user.Email);
                    return RedirectToAction("ConfirmMailSent");
                }
            }
            return View();
        }
        public IActionResult ConfirmMailSent()
        {
            return View();
        }
        public async Task<IActionResult> ConfirmMail(string userId, string Code)
        {
            if (userId == null || Code == null)
            {
                TempData.Add("CodeError", "Code invlaid");
                return View();
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData.Add("UserNotFound", String.Format("We can't find a user"));
                return View();
            }
            var result = await userManager.ConfirmEmailAsync(user, Code);
            if (result.Succeeded)
            {
                TempData.Add("ConfirmSuccess", String.Format("Verification Success! you can login now!"));
                return View();
            }
            TempData.Add("ConfirmFail", String.Format("FAILED"));
            return View();
        }
        [Authorize]
        public IActionResult Logout()
        {
            signInManager.SignOutAsync().Wait();
            SessionService.RemoveToken("key");
            return RedirectToAction("index", "Car");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return View();
            }
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData.Add("WithEmailUserNotFound", "If the user exists, then you will receive an email with a password reset link.");
                return RedirectToAction("ForgotPasswordEmailSend");

            }
            var ForgotPasswordToken = await userManager.GeneratePasswordResetTokenAsync(user);
            var PasswordUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, Code = ForgotPasswordToken });
            smtpService.ResetPasswordMail(PasswordUrl, user.UserName, user.Email);

            return RedirectToAction("ForgotPasswordEmailSend");
        }
        public IActionResult ForgotPasswordEmailSend()
        {
            return View();
        }
        public async Task<IActionResult> ResetPassword(string userId, string Code)
        {
            if (userId == null || Code == null)
            {
                TempData.Add("CodeError", "Code invlaid");
                return View();
            }
            var user = await userManager.FindByIdAsync(userId);
            var model = new ResetPasswordViewModel
            {
                Code = Code,
                Email = user.Email
            };

            return View(model);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                if (resetPasswordViewModel.Password != resetPasswordViewModel.ConfirmPassword)
                {

                    TempData.Add("PasswordsNotMatch", "Your must enter same password value");
                    return View(resetPasswordViewModel);
                }
                var user = await userManager.FindByEmailAsync(resetPasswordViewModel.Email);
                if (user == null)
                {
                    TempData.Add("MailNotFound", String.Format("This mail adress not match with any user"));
                }
                var result = await userManager.ResetPasswordAsync(user, resetPasswordViewModel.Code, resetPasswordViewModel.Password);
                if (result.Succeeded)
                {
                    TempData.Add("ResetPasswordSucceed", "Your password  successfully change!");
                    smtpService.PasswordChanged(user.UserName, user.Email);
                }
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserProfile()
        {
            var user = SessionService.GetValue("key");
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var LoggedUser = await userManager.FindByNameAsync(user);
            if (LoggedUser == null)
            {
                return RedirectToAction("Login");
            }
            var model = new UserInfoViewModel
            {
                username = LoggedUser.UserName,
                Email = LoggedUser.Email,
                IsTwoFactoryEnable = await userManager.GetTwoFactorEnabledAsync(LoggedUser),

            };
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserInfoViewModel userInfoViewModel)
        {
            if (ModelState.IsValid)
            {

                if (userInfoViewModel.NewPassword != userInfoViewModel.ConfirmNewPassword)
                {
                    TempData.Add("PasswordNotMatch", "You must enter same password values");
                    return RedirectToAction("UserProfile");
                }
                var user = await userManager.FindByEmailAsync(userInfoViewModel.Email);
                var result = await userManager.ChangePasswordAsync(user, userInfoViewModel.CurrentPassword, userInfoViewModel.NewPassword);
                if (result.Succeeded)
                {
                    TempData.Add("PasswordChanced", "Your password successfully changed please resing here");
                    return RedirectToAction("Login");
                }
                TempData.Add("CurrentPasswordInvlaid", "Your current password is invlaid");
            }

            return RedirectToAction("UserProfile");
        }
        [HttpPost]
        public async Task<IActionResult> Security(UserInfoViewModel userInfoViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(userInfoViewModel.username);
                var result = await userManager.SetTwoFactorEnabledAsync(user, userInfoViewModel.IsTwoFactoryEnable);
                if (result.Succeeded)
                {
                    TempData.Add("TwoFactorSuccess", "Your two factor enabled with your email.");
                    return RedirectToAction("UserProfile");
                }

            }
            return RedirectToAction("Login");

        }
        public async Task<IActionResult> TwoFactorCode()
        {

            var value = SessionService.GetValue("key");
            var user = await userManager.FindByNameAsync(value);

            if (user == null)
            {
                TempData.Add("NoOneIsHere", "No user found");
                return View();
            }
            var code = await userManager.GenerateTwoFactorTokenAsync(user, "Email");

            if (code != null)
            {

                TempData.Add("TwoFactorCodeSend", "Your two factor authentication code send your mail please check your mail and enter your code here.");
                smtpService.TwoFactorCodeMail(user.UserName, user.Email, code);
            }
            var model = new TwoFactorAuthenticationModel
            {
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TwoFactorCode(TwoFactorAuthenticationModel authenticationModel)
        {

            if (ModelState.IsValid)
            {
                var value = SessionService.GetValue("key");
                var user = await userManager.FindByNameAsync(value);
                var result = await userManager.VerifyTwoFactorTokenAsync(user, "Email", authenticationModel.AuthenticationCode);
                if (result)
                {
                    SessionService.SetValue(user.UserName);
                    var login = await signInManager.TwoFactorSignInAsync("Email", authenticationModel.AuthenticationCode, true, authenticationModel.rememberClient);
                    if (login.Succeeded)
                    {
                        SessionService.SetValue(user.UserName);
                        return RedirectToAction("HomePage", "Management");
                    }

                }
                else{
                    TempData.Add("WrongFactoreCode", "Your two factor code  wrong check your mail and try again.");
                }

            }
            return View();

        }
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = SessionService.GetValue("key");
            var loggedUser = await userManager.FindByNameAsync(user);
            var result = await userManager.DeleteAsync(loggedUser);
            if (result.Succeeded)
            {
                TempData.Add("AccountDeleted", "Your account successfully deleted.");
                return RedirectToAction("index", "Car");

            }
            return RedirectToAction("HomePage", "Management");
        }
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UserAccountActivity(int page=1)
        {
            var pagesize = 10;
            
            var user = SessionService.GetValue("key");
            var loggedUser = await userManager.FindByNameAsync(user);
            var logs = activityLogDal.GetList(filter => filter.Username == loggedUser.UserName);
            var model = new ActivityLogViewModel
            {
                ActivityLogs = logs.Skip((page - 1) * pagesize).Take(pagesize).ToList(),
                PageSize=pagesize,
                CurrentPage=page,
                PageCount=(int)Math.Ceiling(logs.Count/(double)pagesize)
            };


            return View(model);
        }


    }
}
