using System.Security.Claims;
using ExamenNicIan.Models;
using ExamenNicIan.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ExamenNicIan.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userservice)
        {
            _userService = userservice;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
  
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if email is already registered
                if (await _userService.GetUser(user.Email) != null)
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(user);
                }

                // Save the user to the database
                await _userService.Register(user);

                // Redirect to login page or dashboard
                return RedirectToAction("Login");
            }

            return View(user);
        }
        public IActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.Login(model);
                if (user != null)
                {
                    // Create claims for the authenticated user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.FirstName), // Assuming user.Email contains the email
                        // You can add more claims if needed
                    };

                    // Create identity for the user
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Create authentication properties
                    var authProperties = new AuthenticationProperties
                    {
                        // You can configure various properties here
                        IsPersistent = false, // Make the authentication persistent
                    };

                    // Sign in the user
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
