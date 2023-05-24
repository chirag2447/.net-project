using communityWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using BCryptNet = BCrypt.Net.BCrypt;

namespace communityWeb.Controllers.Client
{
    public class RegisterController : Controller
    {
        private readonly ProjectContext _context;

        public RegisterController(ProjectContext context)
        {
            _context = context;
        }
        // GET: RegisterController
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(User user)
        {

            user.RegisteredDate = DateTime.Now;
            var hasPass = BCryptNet.HashPassword(user.Password);
            user.Password = hasPass;
            user.IsActive = true;

            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login", "Register");
        }
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("name");
            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public ActionResult Verify(User user)
        {
            var userr = _context.Users.Where(u=>u.EmailId == user.EmailId).FirstOrDefault();
            
            if(userr != null)
            {
                var storedHashedPassword = userr.Password;
                var isPasswordValid = BCryptNet.Verify(user.Password, storedHashedPassword);

                if (isPasswordValid)
                {
                    Console.WriteLine("valid");
                    HttpContext.Session.SetString("name", userr.Fname);
                    HttpContext.Session.SetInt32("Id", userr.Id);
                    HttpContext.Session.Remove("wrong");
                    return RedirectToAction("index", "home");
                    // Password is valid, proceed with login
                }
                else
                {
                    Console.WriteLine("invalid");
                    return RedirectToAction("login", "register");
                    // Password is invalid, handle authentication failure
                }

            }
            else
            {

                HttpContext.Session.SetString("wrong", "The email or password you've entered is incorrect");
                    ViewBag.wrong = "The email or password you've entered is incorrect";
                    return RedirectToAction("login", "register");
            }

            // Verify the entered password against the stored hash

            
        }
        // GET: RegisterController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RegisterController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RegisterController/Create
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

        // GET: RegisterController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RegisterController/Edit/5
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

        // GET: RegisterController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RegisterController/Delete/5
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
