using Microsoft.AspNetCore.Mvc;

namespace ExamenNicIan.Controllers
{
    public class MapController
    {
        public IActionResult Index()
        {
            var model = new MapModel("myMap");
            return View(model);
        }
    }
}
