using ExamenNicIan.Models;
using ExamenNicIan.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Web.Mvc;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace ExamenNicIan.Controllers
{
    public class MapController :Controller
    {
        private readonly RestaurantService _restaurantService;
        public MapController(RestaurantService restaurantservice )
        {
            _restaurantService = restaurantservice;
        }

        public async Task<IActionResult> Index()
        {
            
            return View();
        }
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.HttpPost]
        public  IActionResult Fetch([FromBody] Location model)
        {

            var restaurants =  _restaurantService.GetRestaurantsFromApi(model.Latitude, model.Longitude).GetAwaiter().GetResult();
            return View("Index", restaurants);
        }
    }
}
