using ExamenNicIan.Models;
using ExamenNicIan.Services;
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
                var restaurants = await _restaurantService.GetRestaurantsFromApi(latitude, longitude);

                return View(restaurants);
            }
        }

    }

