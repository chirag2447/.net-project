using communityWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace communityWeb.Controllers
{
    public class CascadeController : Controller
    {
        private readonly ProjectContext context;

        public CascadeController(ProjectContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult Country()
        {
            var con = context.Countries.ToList();
            return new JsonResult(con);
        }
        public JsonResult State(int id)
        {
            //var con = context.States.Where(x => x.Country.Id == id);
            var con = context.States.ToList();
            return new JsonResult(con);
        }
        public JsonResult City(string id)
        {
            var con = context.Cities.Where(x => x.State.Name == id);
            return new JsonResult(con);
        }
    }
}
