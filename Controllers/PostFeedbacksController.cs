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
    public class PostFeedbacksController : Controller
    {
        private readonly ProjectContext _context;

        public PostFeedbacksController(ProjectContext context)
        {
            _context = context;
        }

        // GET: PostFeedbacks
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            var projectContext = _context.PostFeedbacks.Include(p => p.Post).Include(p => p.User).ToList();
            if (searchString != null)
            {
                projectContext = projectContext.Where(a => a.Post.Title.ToLower().Contains(searchString)||a.User.Fname.ToLower().Contains(searchString)).ToList();
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

            var pagedProducts = new StaticPagedList<PostFeedback>(
                projectContext.Skip((pageNumber - 1) * pageSize).Take(pageSize),
                pageNumber,
                pageSize,
                totalItems);
            ViewBag.CurrentFilter = searchString;
            return View(pagedProducts);
        }

        // GET: PostFeedbacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PostFeedbacks == null)
            {
                return NotFound();
            }

            var postFeedback = await _context.PostFeedbacks
                .Include(p => p.Post)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postFeedback == null)
            {
                return NotFound();
            }

            return View(postFeedback);
        }

        // GET: PostFeedbacks/Create
        public IActionResult Create()
        {
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname");
            return View();
        }

        // POST: PostFeedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PostId,UserId,Review,CreatedDate")] PostFeedback postFeedback)
        {
            if (ModelState.IsValid)
            {
                postFeedback.CreatedDate = DateTime.Now;
                _context.Add(postFeedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title", postFeedback.PostId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname", postFeedback.UserId);
            return View(postFeedback);
        }

        // GET: PostFeedbacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PostFeedbacks == null)
            {
                return NotFound();
            }

            var postFeedback = await _context.PostFeedbacks.FindAsync(id);
            if (postFeedback == null)
            {
                return NotFound();
            }
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title", postFeedback.PostId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname", postFeedback.UserId);
            return View(postFeedback);
        }

        // POST: PostFeedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PostId,UserId,Review,CreatedDate")] PostFeedback postFeedback)
        {
            if (id != postFeedback.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existedFeed = await _context.PostFeedbacks.FindAsync(id);
                    existedFeed.PostId = postFeedback.PostId;
                    existedFeed.UserId = postFeedback.UserId;
                    existedFeed.Review = postFeedback.Review;
                    _context.Update(existedFeed);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostFeedbackExists(postFeedback.Id))
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
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title", postFeedback.PostId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname", postFeedback.UserId);
            return View(postFeedback);
        }

        // GET: PostFeedbacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PostFeedbacks == null)
            {
                return NotFound();
            }

            var postFeedback = await _context.PostFeedbacks
                .Include(p => p.Post)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postFeedback == null)
            {
                return NotFound();
            }

            return View(postFeedback);
        }

        // POST: PostFeedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PostFeedbacks == null)
            {
                return Problem("Entity set 'ProjectContext.PostFeedbacks'  is null.");
            }
            var postFeedback = await _context.PostFeedbacks.FindAsync(id);
            if (postFeedback != null)
            {
                _context.PostFeedbacks.Remove(postFeedback);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostFeedbackExists(int id)
        {
          return _context.PostFeedbacks.Any(e => e.Id == id);
        }
    }
}
