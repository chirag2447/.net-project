using communityWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace communityWeb.Controllers
{
    public class testController : Controller
    {
        private readonly ProjectContext projectDbContext;

        public testController(ProjectContext projectDbContext)
        {
            this.projectDbContext = projectDbContext;
        }
        // GET: testController
        public ActionResult Index()
        {
            return View(projectDbContext.Users.ToList());
        }

        // GET: testController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: testController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: testController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: testController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: testController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: testController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: testController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
