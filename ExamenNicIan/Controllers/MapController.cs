using ExamenNicIan.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamenNicIan.Controllers
{
    public class MapController :Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
