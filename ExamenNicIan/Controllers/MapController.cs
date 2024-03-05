using ExamenNicIan.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamenNicIan.Controllers
{
    public class MapController :Controller
    {
        public IActionResult Index()
        {
            var model = new MapModel("myMap");
            return View(model);
        }
    }
}
