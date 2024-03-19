using ExamenNicIan.Models;
using ExamenNicIan.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExamenNicIan.Controllers
{
    
        public class RestaurantsController : Controller
        {
            private readonly GeoCodingService _geoCodingService;
            private readonly RestaurantService _restaurantService;
            public RestaurantsController(GeoCodingService geoCodingService, RestaurantService restaurantService)
            {
                _geoCodingService = geoCodingService;
                _restaurantService = restaurantService;

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

                // Sort the elements
                Array.Sort(restaurant.elements, (x, y) =>
                {
                    int xCount = CountFilledProperties(x);
                    int yCount = CountFilledProperties(y);

                    return yCount.CompareTo(xCount); // Sort in descending order
                });

                return View(restaurant);
            }

            private int CountFilledProperties(Element element)
            {
                int count = 0;

                if (!string.IsNullOrEmpty(element.tags.name)) count++;
                if (!string.IsNullOrEmpty(element.tags.addrstreet)) count++;
                if (!string.IsNullOrEmpty(element.tags.addrhousenumber)) count++;
                // Add more checks for other properties...

                return count;
            }
        [Authorize]
        public IActionResult Favorites()
        { 
            return View();
        }
    }

    }

