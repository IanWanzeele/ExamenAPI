using ExamenNicIan.Models;
using ExamenNicIan.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExamenNicIan.Controllers
{
    public class MapController :Controller
    {
        private readonly RestaurantService _restaurantService;
        public MapController(RestaurantService restaurantservice)
        {
            _restaurantService = restaurantservice;
        }

        public IActionResult Index()
        {
            
            return View();
        }
        [HttpGet, HttpPost]
        public async Task<IActionResult> Fetch([FromBody] Location model)
        {
            // Make API call to retrieve restaurant data based on latitude and longitude
            var restaurants = await _restaurantService.GetRestaurantsFromApi(model.Latitude, model.Longitude);

            ViewBag.Restaurants = restaurants;


            // Pass the list of restaurants to the view
            return View("Index");
        }
    }
}
