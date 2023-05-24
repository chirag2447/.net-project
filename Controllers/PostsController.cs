using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using communityWeb.Models;
using Microsoft.Extensions.Hosting;
using PagedList;

namespace communityWeb.Controllers
{
    public class PostsController : Controller
    {
        private readonly ProjectContext _context;
        private readonly IWebHostEnvironment hostEnvironment;

        public PostsController(ProjectContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;
        }

        // GET: Posts
        public async Task<IActionResult> Index(string searchString,int?page)
        {
            var projectContext = _context.Posts.Include(p => p.Community).Include(p => p.User).ToList();
            if (searchString != null)
            {
                projectContext = projectContext.Where(a => a.Community.Name.ToLower().Contains(searchString)||a.Title.ToLower().Contains(searchString)).ToList();
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

            var pagedProducts = new StaticPagedList<Post>(
                projectContext.Skip((pageNumber - 1) * pageSize).Take(pageSize),
                pageNumber,
                pageSize,
                totalItems);
            ViewBag.CurrentFilter = searchString;
            return View(pagedProducts);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Community)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["CommunityId"] = new SelectList(_context.Communities, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CommunityId,UserId,Title,Description,Type,FileName,CreatedDate,IsActive")] Post post,IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                post.CreatedDate = DateTime.Now;
                if (photo != null)
                {
                    string filename = photo.FileName;
                    string filepath = Path.Combine(hostEnvironment.WebRootPath, "img", filename);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {

                        await photo.CopyToAsync(stream);
                    }
                    post.FileName = filename;

                }
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CommunityId"] = new SelectList(_context.Communities, "Id", "Name", post.CommunityId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname", post.UserId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["CommunityId"] = new SelectList(_context.Communities, "Id", "Name", post.CommunityId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname", post.UserId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CommunityId,UserId,Title,Description,Type,IsActive")] Post post,IFormFile photo)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            /*if (ModelState.IsValid)
            {*/
            var existedPost = await _context.Posts.FindAsync(id);
            existedPost.CommunityId = post.CommunityId;
            existedPost.UserId = post.UserId;
            existedPost.Title = post.Title;
            existedPost.Description = post.Description;
            existedPost.Type = post.Type;
            existedPost.IsActive = post.IsActive;
                if (photo != null)
                {
                    string filename = photo.FileName;
                    string filepath = Path.Combine(hostEnvironment.WebRootPath, "img", filename);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {

                        await photo.CopyToAsync(stream);
                    }
                    existedPost.FileName = filename;

                }
                try
                {
                    _context.Update(existedPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(existedPost.Id))
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
           /* ViewData["CommunityId"] = new SelectList(_context.Communities, "Id", "Name", post.CommunityId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname", post.UserId);
            return View(post);*/
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Community)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'ProjectContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
          return _context.Posts.Any(e => e.Id == id);
        }
    }
}
