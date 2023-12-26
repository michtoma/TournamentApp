using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mundial.Data;
using Mundial.Models;
using Mundial.ViewModels;

namespace Mundial.Controllers
{
 
    public class AccountController : Controller
    {
        private readonly UserManager<AppUsers> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<AppUsers> userManager,
           SignInManager<AppUsers> signInManager,
            //RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            // _roleManager = roleManager;
            _context = context;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AppUsers.ToList();
            var betts = _context.Bettings.ToList();
            foreach(var user in applicationDbContext)
            {
                foreach(var bet in betts.Where(b => b.UserID == user.Id))
                {
                    user.Points += bet.BetPoints;
                }
            }

            return View(applicationDbContext);
        }
        public IActionResult Login(string? returnUrl = null)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.ReturnUrl = returnUrl ?? Url.Content("~/");
            return View(loginViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(loginViewModel);
                }
            }
            return View(loginViewModel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Register(string? returnUrl = null)
        {

            RegisterViewModel registerViewModel = new RegisterViewModel();
            registerViewModel.ReturnUrl = returnUrl;
            return View(registerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string? returnUrl = null)
        {
            registerViewModel.ReturnUrl = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new AppUsers { Email = registerViewModel.Email, UserName = registerViewModel.UserName };
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                ModelState.AddModelError("Password", "User could not be created. Password not unique enough");
            }
            return View(registerViewModel);
        }
        public async Task<IActionResult> AccessDenied(string url)
        {
            return View(url);
        }
    }
}
