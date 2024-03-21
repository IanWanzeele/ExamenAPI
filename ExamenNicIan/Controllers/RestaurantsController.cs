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
        public IActionResult Favorites()
        { 
            return View();
        }
    }

    }

