using System;
using Microsoft.AspNetCore.Mvc;
using Src.Models;
using Src.Models.CustomizeIdentity;
using Microsoft.AspNetCore.Identity;
using Src.Models.ViewModels.Register;
using System.Threading.Tasks;
using Src.Helpers;
using System.Collections.Generic;
using Src.Models.ViewModels.Login;

namespace Src.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly SignInManager<ApplicationUser> _signinManger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(AppDbContext context,
         UserManager<ApplicationUser> userManger,
         SignInManager<ApplicationUser> sginInManger,
         RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManger = userManger;
            _signinManger = sginInManger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (_signinManger.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
            else
                return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm model)
        {

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManger.FindByNameAsync(model.UserName);

            if (user != null)
            {
                var result = await _signinManger.PasswordSignInAsync(user, model.Password, model.RemmberMe, false);

                if (result.Succeeded)
                    return RedirectToAction("Index", "AppointMent");
                else if (result.IsLockedOut)
                    return Content("Your Account Is Locked Out... For Multiple attempts...");
                else if (result.IsNotAllowed)
                    return Content("You do not have a permision to Login at this Time");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid error Login Error");
                return View(model);
            }


            return NoContent();



        }



        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (!_roleManager.RoleExistsAsync(Helper.Admin).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Admin });
                await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Doctor });
                await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Patient });
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVm model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                EmailConfirmed =true
            };

            var result = await _userManger.CreateAsync(user, model.Password);


            if (result.Succeeded)
            {

                await _userManger.AddToRoleAsync(user, model.RoleName);
                await _signinManger.SignInAsync(user, false);
                return RedirectToAction("Index");
            }
            else if (result.Errors != null)
            {
                foreach (var errors in result.Errors)
                    ModelState.AddModelError(string.Empty, errors.Description);


                return View(model);
            }
            else
                return Content("Error on server side");
        }




        [HttpPost]
        public async Task<IActionResult> LoggOut()
        {
            await _signinManger.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

    }
}