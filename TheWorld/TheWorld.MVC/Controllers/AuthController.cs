using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.MVC.Controllers
{
    public class AuthController: Controller
    {
        /// <summary>
        /// The signInManager.
        /// </summary>
        private SignInManager<WorldUser> signInManager;

        /// <summary>
        /// The user.
        /// </summary>
        private UserManager<WorldUser> localUserManager;

        public AuthController(SignInManager<WorldUser> inManager, UserManager<WorldUser> userManager)
        {
            this.signInManager = inManager;
            this.localUserManager = userManager;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("GetJs", "TripsMvc");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await this.signInManager.PasswordSignInAsync(loginViewModel.UserName,
                    loginViewModel.Password,
                    true, false);

                if (signInResult.Succeeded)
                {
                    if (string.IsNullOrWhiteSpace(returnUrl))
                        return RedirectToAction("GetJs", "TripsMvc");
                    else
                        return Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "UserName or Password incorect");
                }
            }

            return View();
        }

        //[HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = new WorldUser
                {
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Email
                };
                var registerResult = await this.localUserManager.CreateAsync(user,
                    registerViewModel.Password);

                if (registerResult.Succeeded)
                {
                    if (string.IsNullOrWhiteSpace(returnUrl))
                        return RedirectToAction("GetJs", "TripsMvc");
                    else
                        return Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Couldn't Create the User");
                }
            }

            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                await this.signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "TripsMvc");
        }
    }
}
