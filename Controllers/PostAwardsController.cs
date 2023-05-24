using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using communityWeb.Models;
using PagedList;

namespace communityWeb.Controllers
{
    public class PostAwardsController : Controller
    {
        private readonly ProjectContext _context;

        public PostAwardsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: PostAwards
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            var projectContext = _context.PostAwards.Include(p => p.Award).Include(p => p.Post).Include(p => p.User).ToList();
            if (searchString != null)
            {
                projectContext = projectContext.Where(a => a.Post.Title.ToLower().Contains(searchString)||a.Award.Name.ToLower().Contains(searchString)).ToList();
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
            var pagedProducts = new StaticPagedList<PostAward>(
                projectContext.Skip((pageNumber - 1) * pageSize).Take(pageSize),
                pageNumber,
                pageSize,
                totalItems);
            ViewBag.CurrentFilter = searchString;
            return View(pagedProducts);
        }

        // GET: PostAwards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PostAwards == null)
            {
                return NotFound();
            }

            var postAward = await _context.PostAwards
                .Include(p => p.Award)
                .Include(p => p.Post)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postAward == null)
            {
                return NotFound();
            }

            return View(postAward);
        }

        // GET: PostAwards/Create
        public IActionResult Create()
        {
            ViewData["AwardId"] = new SelectList(_context.Awards, "Id", "Name");
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname");
            return View();
        }

        // POST: PostAwards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PostId,AwardId,UserId,GivenDate")] PostAward postAward)
        {
            if (ModelState.IsValid)
            {
                postAward.GivenDate = DateTime.Now;
                _context.Add(postAward);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AwardId"] = new SelectList(_context.Awards, "Id", "Name", postAward.AwardId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title", postAward.PostId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname", postAward.UserId);
            return View(postAward);
        }

        // GET: PostAwards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PostAwards == null)
            {
                return NotFound();
            }

            var postAward = await _context.PostAwards.FindAsync(id);
            if (postAward == null)
            {
                return NotFound();
            }
            ViewData["AwardId"] = new SelectList(_context.Awards, "Id", "Name", postAward.AwardId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title", postAward.PostId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname", postAward.UserId);
            return View(postAward);
        }

        // POST: PostAwards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PostId,AwardId,UserId,GivenDate")] PostAward postAward)
        {
            if (id != postAward.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existedPa = await _context.PostAwards.FindAsync(id);
                    existedPa.AwardId = postAward.AwardId;
                    existedPa.PostId = postAward.PostId;
                    existedPa.UserId = postAward.UserId;
                    _context.Update(existedPa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostAwardExists(postAward.Id))
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
            ViewData["AwardId"] = new SelectList(_context.Awards, "Id", "Name", postAward.AwardId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title", postAward.PostId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname", postAward.UserId);
            return View(postAward);
        }

        // GET: PostAwards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PostAwards == null)
            {
                return NotFound();
            }

            var postAward = await _context.PostAwards
                .Include(p => p.Award)
                .Include(p => p.Post)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postAward == null)
            {
                return NotFound();
            }

            return View(postAward);
        }

        // POST: PostAwards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PostAwards == null)
            {
                return Problem("Entity set 'ProjectContext.PostAwards'  is null.");
            }
            var postAward = await _context.PostAwards.FindAsync(id);
            if (postAward != null)
            {
                _context.PostAwards.Remove(postAward);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostAwardExists(int id)
        {
          return _context.PostAwards.Any(e => e.Id == id);
        }
    }
}
