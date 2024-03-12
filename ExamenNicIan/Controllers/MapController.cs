using ExamenNicIan.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExamenNicIan.Controllers
{
    public class MapController :Controller
    {
        [HttpGet,HttpPost]
        public IActionResult Index()
        {
            if (TempData["restaurantsJson"] != null)
            {
                var restaurants = JsonConvert.DeserializeObject<Restaurant>(TempData["restaurantsJson"].ToString());
                return View(restaurants);
            }

            return View();
        }

    }
}
