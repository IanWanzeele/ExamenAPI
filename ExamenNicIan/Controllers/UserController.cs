using ExamenNicIan.Models;
using ExamenNicIan.Services;
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

        // POST: /User/Login
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (ModelState.IsValid)
            {
                var model = await _userService.Login(user.Email, user.Password);
                if (model != null)
                {
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                    return View(user);
                }
            }

            return View(user);
        }
    }
}
