using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioAppUX.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly IEmailService _emailService;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            // _emailService = emailService;
            CreateRolesAndUsers().Wait();
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            if (Url.IsLocalUrl(ReturnUrl))
            {
                _ = ReturnUrl.ToLower().Split("/", StringSplitOptions.RemoveEmptyEntries);
                return RedirectToAction(ReturnUrl);
            }
            else
                return RedirectToAction("index", "home");
        }

        private async Task CreateRolesAndUsers()
        {
            bool x = await _roleManager.RoleExistsAsync("PhysicalTherapist");
            if (!x)
            {
                var role = new IdentityRole
                {
                    Name = "PhysicalTherapist"
                };
                await _roleManager.CreateAsync(role);

                var user = new IdentityUser
                {
                    UserName = "defaultTherapist",
                    Email = "default@therapist.com"
                };

                string userPass = "D3f@ultTheR@p1st";

                IdentityResult newUser = await _userManager.CreateAsync(user, userPass);

                if (newUser.Succeeded)
                    await _userManager.AddToRoleAsync(user, "PhysicalTherapist");
            }

            x = await _roleManager.RoleExistsAsync("Intern");
            if (!x)
            {
                var role = new IdentityRole
                {
                    Name = "Intern"
                };
                await _roleManager.CreateAsync(role);

                var user = new IdentityUser
                {
                    UserName = "defaultStudent",
                    Email = "default@student.com"
                };

                string userPass = "D3f@ultSt0d3nT";

                IdentityResult newUser = await _userManager.CreateAsync(user, userPass);

                if (newUser.Succeeded)
                    await _userManager.AddToRoleAsync(user, "Intern");
            }

            x = await _roleManager.RoleExistsAsync("Patient");
            if (!x)
            {
                var role = new IdentityRole
                {
                    Name = "Patient"
                };
                await _roleManager.CreateAsync(role);

                var user = new IdentityUser
                {
                    UserName = "defaultPatient",
                    Email = "default@patient.com"
                };

                string userPass = "D3f@ultP@t1enT";

                IdentityResult newUser = await _userManager.CreateAsync(user, userPass);

                if (newUser.Succeeded)
                    await _userManager.AddToRoleAsync(user, "Patient");
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                //sign in
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (signInResult.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Patient"))
                        return RedirectToAction("PatientHome", "Home");
                    else if (roles.Contains("PhysicalTherapist"))
                        return RedirectToAction("FysioHome", "Home");
                    return RedirectToAction("index", "home");
                }
            }

            return View();
        }

        public IActionResult ComfirmAccount(string link)
        {
            return View("ComfirmAccount", link);
        }

        [Route("Account/VerifyEmail/{userId}/{code}")]
        [Route("Account/VerifyEmail/userId/code")]
        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return BadRequest();

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return View();
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password, string comfirmPass)
        {
            if (password == comfirmPass)
            {
                var user = new IdentityUser
                {
                    UserName = username,
                    Email = email
                };

                var restult = await _userManager.CreateAsync(user, password);

                if (restult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Patient");
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var link = Url.Action(nameof(VerifyEmail), "Account", new { userId = user.Id, code });
                    return ComfirmAccount(link);
                }
                else 
                {
                    ModelState.AddModelError("Password", "passwords must is invalid");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("Password", "passwords don't match");
                return View();
            }

            return RedirectToAction("index", "home");
        }
    }
}
