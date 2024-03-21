using System.Security.Claims;
using ExamenNicIan.Core;
using ExamenNicIan.Models;
using ExamenNicIan.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamenNicIan.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly UserDbContext _userDbContext;

        public UserController(UserService userservice, UserDbContext userDbContext)
        {
            _userService = userservice;
            _userDbContext = userDbContext;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                var loginModel = new Login { Email = user.Email, Password = user.Password };
                return await Login(loginModel);

            }

            return View(user);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
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
        public IActionResult Edit()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId))
            {
                // Handle the case where the user ID cannot be parsed to an integer
                // For example, redirect to the home page or display an error message
                return RedirectToAction("Index", "Home");
            }

            // Retrieve the user from the database based on the ID
            var user = _userDbContext.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                // If the user is not found, redirect to the home page or display an error message
                return RedirectToAction("Index", "Home");
            }

            // Pass the user object to the view for editing
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId))
            {
                // Handle the case where the user ID cannot be parsed to an integer
                // For example, redirect to the home page or display an error message
                return RedirectToAction("Index", "Home");
            }
            var dbUser = _userDbContext.Users.FirstOrDefault(u => u.UserId == userId);

            if (dbUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Update user properties
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Email = user.Email;
            // Add other properties as needed

            _userDbContext.SaveChanges();
            var identity = (ClaimsIdentity)User.Identity;
            var nameClaim = identity.FindFirst(ClaimTypes.Name);
            if (nameClaim != null)
            {
                identity.RemoveClaim(nameClaim);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.FirstName));
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Sign the user back in with updated claims
            var newIdentity = new ClaimsIdentity(identity.Claims, identity.AuthenticationType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(newIdentity));


            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        [HttpPost("/[controller]/Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        {
            // Get the currently logged-in user's ID
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Find the user by ID
            var user = await _userDbContext.Users.FindAsync(userId);


            if (user is null)
            {
                // If the user is not found, redirect to the home page
                return RedirectToAction("Index", "Home");
            }
            var favoritesToRemove = _userDbContext.Favorites.Where(f => f.UserID == userId);
            _userDbContext.Favorites.RemoveRange(favoritesToRemove);

            // Remove the user
            _userDbContext.Users.Remove(user);
            await _userDbContext.SaveChangesAsync();
            HttpContext.SignOutAsync();

            // Redirect to the home page
            return RedirectToAction("Index", "Home");
        }

    }
}
