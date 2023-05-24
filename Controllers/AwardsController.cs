using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using communityWeb.Models;
using Microsoft.Extensions.Hosting;
using System.Net;
using PagedList;

namespace communityWeb.Controllers
{
    public class AwardsController : Controller
    {
        private readonly ProjectContext _context;
        private readonly IWebHostEnvironment HostEnvironment;

        public AwardsController(ProjectContext context,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            HostEnvironment = hostEnvironment;
        }

        // GET: Awards
        public async Task<IActionResult> Index(string searchString,int? page)
        {
            var award = _context.Awards.ToList();
            if (searchString != null)
            {
                award = award.Where(a=> a.Name.ToLower().Contains(searchString)).ToList();
            }
            int pageSize = 10;
            ViewBag.pageSize = pageSize;
            if (page <= 0)
            {
                page = 1;
            }
            int pageNumber = (page ?? 1);
            ViewBag.page = pageNumber;
            int totalItems = award.Count();
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

            var pagedProducts = new StaticPagedList<Award>(
                award.Skip((pageNumber - 1) * pageSize).Take(pageSize),
                pageNumber,
                pageSize,
                totalItems);
            ViewBag.CurrentFilter = searchString;
              return View(pagedProducts);
        }

        // GET: Awards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Awards == null)
            {
                return NotFound();
            }

            var award = await _context.Awards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (award == null)
            {
                return NotFound();
            }

            return View(award);
        }

        // GET: Awards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Awards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
     /*   public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Image")] Award award)
        {
            if (ModelState.IsValid)
            {
                _context.Add(award);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(award);
        }*/
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price")] Award award,IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    string filename = photo.FileName;
                    string filepath = Path.Combine(HostEnvironment.WebRootPath, "img", filename);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {

                        await photo.CopyToAsync(stream);
                    }
                    award.Image = filename;

                }
            }
           

            _context.Add(award);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Awards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Awards == null)
            {
                return NotFound();
            }

            var award = await _context.Awards.FindAsync(id);
            if (award == null)
            {
                return NotFound();
            }
            return View(award);
        }

        // POST: Awards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       /* public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Image")] Award award)
        {
            if (id != award.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(award);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AwardExists(award.Id))
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
            return View(award);
        }*/
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price")] Award award, IFormFile photo)
        {
            if (id != award.Id)
            {
                return NotFound();
            }
          /*  if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                Console.WriteLine("-------------"+message);
            }*/
           /* if (ModelState.IsValid)
            {*/
            var existingAward = await _context.Awards.FindAsync(id);
                existingAward.Name = award.Name;
                existingAward.Description = award.Description;
                existingAward.Price = award.Price;
                if (photo != null)
                {
                    string filename = photo.FileName;
                    string filepath = Path.Combine(HostEnvironment.WebRootPath, "img", filename);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {

                        await photo.CopyToAsync(stream);
                    }
/*                    award.Image = filename;
*/                    

                    // Update the fields of the retrieved record with the values from the user parameter
                   
                   
                        existingAward.Image = filename;
                }
                    _context.Update(existingAward);
                    await _context.SaveChangesAsync();
            /*}*/

           
           
            return RedirectToAction(nameof(Index));
        }

        // GET: Awards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Awards == null)
            {
                return NotFound();
            }

            var award = await _context.Awards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (award == null)
            {
                return NotFound();
            }

            return View(award);
        }

        // POST: Awards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Awards == null)
            {
                return Problem("Entity set 'ProjectContext.Awards'  is null.");
            }
            var award = await _context.Awards.FindAsync(id);
            if (award != null)
            {
                _context.Awards.Remove(award);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AwardExists(int id)
        {
          return _context.Awards.Any(e => e.Id == id);
        }
    }
}
