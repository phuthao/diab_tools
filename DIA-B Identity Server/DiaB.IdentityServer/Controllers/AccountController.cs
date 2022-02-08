using System.Collections.Generic;
using System.Security.Claims;
using DiaB.IdentityServer.Models;
using DiaB.IdentityServer.Services;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DiaB.IdentityServer.Constants;
using IdentityServer4;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace DiaB.IdentityServer.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            IAccountService accountService,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _accountService = accountService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var isPasswordCorrect = await CheckPassword(request.CurrentPassword);

            if (!isPasswordCorrect)
            {
                ViewBag.IncorrectPassword = true;

                return View();
            }

            var result = await _accountService.ChangePassword(User.GetSubjectId(), request.NewPassword);

            if (result != null && result.Succeeded)
            {
                var user = await _userManager.FindByIdAsync(User.GetSubjectId());

                await SignIn(user);

                var returnUrl = HttpContext.Request.Query["returnUrl"];

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("login");
            }

            return BadRequest(result.Errors);
        }

        private async Task<bool> CheckPassword(string password)
        {
            var user = await _userManager.FindByIdAsync(User.GetSubjectId());
            var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            return result == PasswordVerificationResult.Success;
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgetPassword([FromForm] ResetPasswordEmailModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user is null)
            {
                ModelState.AddModelError("EmailNotFound", string.Empty);

                return View();
            }

            await _accountService.ResetPassword(user.Id);

            return RedirectToAction("LoginEmail", new LoginEmailModel{Email = request.Email});
        }

        [AllowAnonymous]
        public IActionResult LoginEmail(string email)
        {
            return View(new LoginEmailModel{Email = email});
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginEmail(LoginEmailModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user is null)
            {
                return NotFound();
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, true, true);

            if (result.Succeeded)
            {
                await SignIn(user);
            }

            return RedirectToAction("ChangePassword");
        }

        private async Task SignIn(User user)
        {
            var isuser = new IdentityServerUser(user.Id)
            {
                DisplayName = user.UserName
            };

            if (user.MustChangePassword)
            {
                isuser.AdditionalClaims = new List<Claim>
                {
                    new Claim(Claims.MustChangePassword, string.Empty)
                };
            }

            await HttpContext.SignInAsync(isuser, null);
        }
    }
}
