using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public UserAccountController(UserManager<ApplicationUser> userManager,
                                     SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public UserManager<ApplicationUser> UserManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApplicationUser { 
                    UserName = user.UserName, 
                    Email = user.Email,
                    City = user.City
                };
                //create a new user
                var result = await userManager.CreateAsync(newUser, user.Password);

                if (result.Succeeded)
                {
                    if(signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUser", "Administration");
                    }
                    //if user is successfully registered, then we use that username to sign in
                    await signInManager.SignInAsync(newUser, isPersistent: false);
                    return RedirectToAction("Index", "Employee");
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Employee");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser u = await userManager.FindByEmailAsync(user.Email);
                var result = await signInManager.PasswordSignInAsync(u,
                                        user.Password, user.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        //to prevent attacker's malicious acts
                        //return LocalRedirect(returnUrl);
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Employee");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(user);
        }

        [HttpPost]
        [HttpGet]
        //this method for the client side validation to check email address has been used
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if(user == null)
            {
                //use Json for return value: as jquery validate method use ajax to call this server-side function
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already registered.");
            }
        }
    }   
}
