using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using communityWeb.Models;
using Microsoft.Data.SqlClient;
using PagedList;

namespace communityWeb.Controllers
{
    public class AdminsController : Controller
    {
        private readonly ProjectContext _context;

        public AdminsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Admins
        /*public async Task<IActionResult> Index(string searchString)
        {
           *//* ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";*//*
            ViewBag.CurrentFilter = searchString;

            var admins = await _context.Admins.ToListAsync();
            if (searchString != null)
            {
                admins = admins.Where(a => a.UName.Contains(searchString) || a.Name.Contains(searchString)).ToList();
            }
           *//* switch (sortOrder)
            {
                case "name_desc":
                    admins = admins.OrderByDescending(a => a.UName).ToList();
                    break;

                default:

                    break;
            }*//*

            return View(admins);
        }*/
        public async Task<IActionResult> Index(string searchString,int?page)
        {
           /* ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";*/
            

            var admins = await _context.Admins.ToListAsync();
            if (searchString != null)
            {
                admins = admins.Where(a => a.UName.Contains(searchString) || a.Name.Contains(searchString)||a.EmailId.Contains(searchString)).ToList();
            }
            /* switch (sortOrder)
             {
                 case "name_desc":
                     admins = admins.OrderByDescending(a => a.UName).ToList();
                     break;

                 default:

                     break;
             }*/

            int pageSize = 10;
            ViewBag.pageSize = pageSize;
            if (page <= 0)
            {
                page = 1;
            }
            int pageNumber = (page ?? 1);
            ViewBag.page = pageNumber;
            int totalItems = admins.Count();
            ViewData["totalItems"] = totalItems;
            float b = totalItems / pageSize;
            if (totalItems % pageSize == 0)
            {

                ViewBag.totalPage = b;

            }
            else
            {

                ViewBag.totalPage = b + 1;
            }

            var pagedProducts = new StaticPagedList<Admin>(
                admins.Skip((pageNumber - 1) * pageSize).Take(pageSize),
                pageNumber,
                pageSize,
                totalItems);
            ViewBag.CurrentFilter = searchString;
            return View(pagedProducts);
        }

        /*public IActionResult Index(int? page )
        {
            int pageSize = 5;
            if(page <= 0)
            {
                page = 1;
            }
            int pageNumber = (page ?? 1);
            ViewBag.page = pageNumber;
            List<Admin> products = _context.Admins.ToList();
            int totalItems = products.Count();
            float b = totalItems / pageSize;
            var a =(int)Math.Ceiling(b );
          
            ViewBag.totalPage =a+1 ;

            var pagedProducts = new StaticPagedList<Admin>(
                products.Skip((pageNumber - 1) * pageSize).Take(pageSize),
                pageNumber,
                pageSize,
                totalItems);

            return View(pagedProducts);
        }*/

        // GET: Admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Admins == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: Admins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UName,Name,EmailId,Password,IsActive")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }

        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Admins == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UName,Name,EmailId,Password,IsActive")] Admin admin)
        {
            if (id != admin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }

        // GET: Admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Admins == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Admins == null)
            {
                return Problem("Entity set 'ProjectContext.Admins'  is null.");
            }
            var admin = await _context.Admins.FindAsync(id);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(int id)
        {
          return _context.Admins.Any(e => e.Id == id);
        }
    }
}
