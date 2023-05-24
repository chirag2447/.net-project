using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using communityWeb.Models;
using System.Net;
using PagedList;

namespace communityWeb.Controllers
{
    public class CommunitiesController : Controller
    {
        private readonly ProjectContext _context;
        private readonly IWebHostEnvironment HostEnvironment;

        public CommunitiesController(ProjectContext context,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.HostEnvironment = hostEnvironment;
        }

        // GET: Communities
        public async Task<IActionResult> Index(string searchString,int?page)
        {
            var projectContext = _context.Communities.Include(c => c.Owner).ToList();
            if (searchString != null)
            {
                projectContext = projectContext.Where(a => a.Name.ToLower().Contains(searchString)||a.Description.ToLower().Contains(searchString)).ToList();
            }
            int pageSize = 10;
            ViewBag.pageSize = pageSize;
            if (page <= 0)
            {
                page = 1;
            }
            int pageNumber = (page ?? 1);
            ViewBag.page = pageNumber;
            int totalItems = projectContext.Count();
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
            var pagedProducts = new StaticPagedList<Community>(
                projectContext.Skip((pageNumber - 1) * pageSize).Take(pageSize),
                pageNumber,
                pageSize,
                totalItems);
            ViewBag.CurrentFilter = searchString;
            return View(pagedProducts);
        }

        // GET: Communities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Communities == null)
            {
                return NotFound();
            }

            var community = await _context.Communities
                .Include(c => c.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (community == null)
            {
                return NotFound();
            }

            return View(community);
        }

        // GET: Communities/Create
        public IActionResult Create()
        {
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Fname");
            return View();
        }

        // POST: Communities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,OwnerId,IsApproved,CreatedDate")] Community community,IFormFile photo)
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
                    community.Logo = filename;

                }
                community.CreatedDate = DateTime.Now;
                _context.Add(community);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Fname", community.OwnerId);
            return View(community);
        }

        // GET: Communities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Communities == null)
            {
                return NotFound();
            }

            var community = await _context.Communities.FindAsync(id);
            if (community == null)
            {
                return NotFound();
            }
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Fname", community.OwnerId);
            return View(community);
        }

        // POST: Communities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,OwnerId,IsApproved")] Community community,IFormFile photo)
        {
            if (id != community.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                await Console.Out.WriteLineAsync("----------------------"+message);
            }
            /*if (ModelState.IsValid)
            {*/
                var existingCom = await _context.Communities.FindAsync(id);
                existingCom.Name = community.Name;
                existingCom.Description = community.Description;
                existingCom.OwnerId = community.OwnerId;
                existingCom.IsApproved = community.IsApproved;
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


                    existingCom.Logo = filename;
                }
                try
                {
                    
                    _context.Update(existingCom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommunityExists(community.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            /*}*/
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", community.OwnerId);
            return View(community);
        }

        // GET: Communities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Communities == null)
            {
                return NotFound();
            }

            var community = await _context.Communities
                .Include(c => c.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (community == null)
            {
                return NotFound();
            }

            return View(community);
        }

        // POST: Communities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Communities == null)
            {
                return Problem("Entity set 'ProjectContext.Communities'  is null.");
            }
            var community = await _context.Communities.FindAsync(id);
            if (community != null)
            {
                _context.Communities.Remove(community);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommunityExists(int id)
        {
          return _context.Communities.Any(e => e.Id == id);
        }
    }
}
