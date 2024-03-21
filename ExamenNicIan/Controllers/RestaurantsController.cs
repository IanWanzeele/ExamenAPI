using System.Security.Claims;
using ExamenNicIan.Core;
using ExamenNicIan.Models;
using ExamenNicIan.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExamenNicIan.Controllers
{

    public class RestaurantsController : Controller
    {
        private readonly GeoCodingService _geoCodingService;
        private readonly RestaurantService _restaurantService;
        private readonly UserDbContext _userDbContext;
        public RestaurantsController(GeoCodingService geoCodingService, RestaurantService restaurantService, UserDbContext userDbContext)
        {
            _geoCodingService = geoCodingService;
            _restaurantService = restaurantService;
            _userDbContext = userDbContext;
        }

        [HttpPost]
        public async Task<ActionResult> Search(string query)
        {
            var (latitude, longitude) = await _geoCodingService.GeoLocation(query);

            if (latitude == 0 && longitude == 0)
            {
                return View("Error");
            }

            // Fetch restaurants based on obtained latitude and longitude
            var restaurant = await _restaurantService.GetRestaurantsFromApi(latitude, longitude);


            Array.Sort(restaurant.elements, (x, y) =>
            {
                int xCount = CountFilledProperties(x);
                int yCount = CountFilledProperties(y);


                bool xHasEmptyAddress = string.IsNullOrEmpty(x.tags.addrstreet) || string.IsNullOrEmpty(x.tags.addrcity) || string.IsNullOrEmpty(x.tags.addrpostcode);
                bool yHasEmptyAddress = string.IsNullOrEmpty(y.tags.addrstreet) || string.IsNullOrEmpty(y.tags.addrcity) || string.IsNullOrEmpty(y.tags.addrpostcode);


                if (xHasEmptyAddress && yHasEmptyAddress)
                {
                    return 0;
                }

                else if (xHasEmptyAddress)
                {
                    return 1;
                }

                else if (yHasEmptyAddress)
                {
                    return -1;
                }

                else
                {
                    return yCount.CompareTo(xCount);
                }
            });

            return View(restaurant);
        }

        private int CountFilledProperties(Element element)
        {
            int count = 0;

            if (!string.IsNullOrEmpty(element.tags.name)) count++;
            if (!string.IsNullOrEmpty(element.tags.addrstreet)) count++;
            if (!string.IsNullOrEmpty(element.tags.addrhousenumber)) count++;
            if (!string.IsNullOrEmpty(element.tags.cuisine)) count++;
            if (!string.IsNullOrEmpty(element.tags.phone)) count++;
            if (!string.IsNullOrEmpty(element.tags.website)) count++;
            if (!string.IsNullOrEmpty(element.tags.addrcity)) count++;
            if (!string.IsNullOrEmpty(element.tags.addrpostcode)) count++;

            return count;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ToFavorites(long restaurantId, string restaurantName)
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                return BadRequest("Invalid user ID.");
            }

            var existingFavorite = await _userDbContext.Favorites
                .FirstOrDefaultAsync(fr => fr.RestaurantID == restaurantId && fr.UserID == userId);

            if (existingFavorite != null)
            {
                _userDbContext.Favorites.Remove((existingFavorite));
                await _userDbContext.SaveChangesAsync();
                return RedirectToAction("Favorites");
            }
            else
            {
                var favorite = new Favorite
                {
                    UserID = userId,
                    RestaurantID = restaurantId,
                    RestaurantName = restaurantName
                };

                _userDbContext.Favorites.Add(favorite);
                await _userDbContext.SaveChangesAsync();
                return NoContent();
            }

        }
        [Authorize]
        public async Task<IActionResult> Favorites()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var favoriteRestaurantIds = await _userDbContext.Favorites
                .Where(fr => fr.UserID == userId)
                .Select(fr => fr.RestaurantID)
                .ToListAsync();

            var favoriteRestaurants = new Restaurant
            {
                elements = new Element[favoriteRestaurantIds.Count]
            };
            for (int i = 0; i < favoriteRestaurantIds.Count; i++)
            {
                var restaurantId = favoriteRestaurantIds[i];
                var restaurant = await _restaurantService.GetRestaurantById(restaurantId);
                if (restaurant != null)
                {
                    favoriteRestaurants.elements[i] = restaurant.elements.FirstOrDefault();
                }
            }

            return View(favoriteRestaurants);
        }

        [HttpGet]
        public async Task<JsonResult> IsFavorite(long restaurantId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isFavorite = await _userDbContext.Favorites.AnyAsync(fr => fr.RestaurantID == restaurantId && fr.UserID == userId);
            return Json(isFavorite);
        }

    }

}

