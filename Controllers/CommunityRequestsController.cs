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
    public class CommunityRequestsController : Controller
    {
        private readonly ProjectContext _context;

        public CommunityRequestsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: CommunityRequests
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            var projectContext = _context.CommunityRequests.Include(c => c.Community).Include(c => c.SentByUser).Include(c => c.User).ToList();
            if (searchString != null)
            {
                projectContext = projectContext.Where(a => a.SentByUser.Fname.ToLower().Contains(searchString)||a.Community.Name.ToLower().Contains(searchString)).ToList();
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
            var pagedProducts = new StaticPagedList<CommunityRequest>(
                projectContext.Skip((pageNumber - 1) * pageSize).Take(pageSize),
                pageNumber,
                pageSize,
                totalItems);
            ViewBag.CurrentFilter = searchString;
            return View(pagedProducts);
        }

        // GET: CommunityRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CommunityRequests == null)
            {
                return NotFound();
            }

            var communityRequest = await _context.CommunityRequests
                .Include(c => c.Community)
                .Include(c => c.SentByUser)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (communityRequest == null)
            {
                return NotFound();
            }

            return View(communityRequest);
        }

        // GET: CommunityRequests/Create
        public IActionResult Create()
        {
            ViewData["CommunityId"] = new SelectList(_context.Communities, "Id", "Name");
            ViewData["SentByUserId"] = new SelectList(_context.Users, "Id", "Fname");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname");
            return View();
        }

        // POST: CommunityRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CommunityId,UserId,SentByUserId,Status")] CommunityRequest communityRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(communityRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CommunityId"] = new SelectList(_context.Communities, "Id", "Id", communityRequest.CommunityId);
            ViewData["SentByUserId"] = new SelectList(_context.Users, "Id", "Id", communityRequest.SentByUserId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", communityRequest.UserId);
            return View(communityRequest);
        }

        // GET: CommunityRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CommunityRequests == null)
            {
                return NotFound();
            }

            var communityRequest = await _context.CommunityRequests.FindAsync(id);
            if (communityRequest == null)
            {
                return NotFound();
            }
            ViewData["CommunityId"] = new SelectList(_context.Communities, "Id", "Name", communityRequest.CommunityId);
            ViewData["SentByUserId"] = new SelectList(_context.Users, "Id", "Fname", communityRequest.SentByUserId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname", communityRequest.UserId);
            return View(communityRequest);
        }

        // POST: CommunityRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CommunityId,UserId,SentByUserId,Status")] CommunityRequest communityRequest)
        {
            if (id != communityRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                try
                {
                    _context.Update(communityRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommunityRequestExists(communityRequest.Id))
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
            ViewData["CommunityId"] = new SelectList(_context.Communities, "Id", "Id", communityRequest.CommunityId);
            ViewData["SentByUserId"] = new SelectList(_context.Users, "Id", "Id", communityRequest.SentByUserId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", communityRequest.UserId);
            return View(communityRequest);
        }

        // GET: CommunityRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CommunityRequests == null)
            {
                return NotFound();
            }

            var communityRequest = await _context.CommunityRequests
                .Include(c => c.Community)
                .Include(c => c.SentByUser)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (communityRequest == null)
            {
                return NotFound();
            }

            return View(communityRequest);
        }

        // POST: CommunityRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CommunityRequests == null)
            {
                return Problem("Entity set 'ProjectContext.CommunityRequests'  is null.");
            }
            var communityRequest = await _context.CommunityRequests.FindAsync(id);
            if (communityRequest != null)
            {
                _context.CommunityRequests.Remove(communityRequest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommunityRequestExists(int id)
        {
          return _context.CommunityRequests.Any(e => e.Id == id);
        }
    }
}
