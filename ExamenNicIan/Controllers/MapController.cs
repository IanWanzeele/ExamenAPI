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
            // Api Call om de restaurants op te halen voor map
            var restaurants = await _restaurantService.GetRestaurantsFromApi(model.Latitude, model.Longitude);

            ViewBag.Restaurants = restaurants;


            
            return View("Index");
        }
    }
}
