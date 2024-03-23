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
                // Controleren of email reeds bestaat
                if (await _userService.GetUser(user.Email) != null)
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(user);
                }

                // Opslaan van user
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
                    
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.FirstName), 
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    };

                    
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        
                        IsPersistent = false, // Indien op true blijf je altijd aangemeld
                    };

                    // Aanmelden van de user
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
             
                return RedirectToAction("Index", "Home");
            }

            var user = _userDbContext.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
            
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId))
            {
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
         

            _userDbContext.SaveChanges();
            var identity = (ClaimsIdentity)User.Identity;
            var nameClaim = identity.FindFirst(ClaimTypes.Name);
            if (nameClaim != null)
            {
                identity.RemoveClaim(nameClaim);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.FirstName));
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

      
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
        
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        
            var user = await _userDbContext.Users.FindAsync(userId);


            if (user is null)
            {
           
                return RedirectToAction("Index", "Home");
            }
            var favoritesToRemove = _userDbContext.Favorites.Where(f => f.UserID == userId);
            _userDbContext.Favorites.RemoveRange(favoritesToRemove);

            // Verwijder de gebruiker
            _userDbContext.Users.Remove(user);
            await _userDbContext.SaveChangesAsync();
            HttpContext.SignOutAsync();

            // Redirect naar home
            return RedirectToAction("Index", "Home");
        }

    }
}
