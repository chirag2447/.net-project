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
    public class FriendRequestsController : Controller
    {
        private readonly ProjectContext _context;

        public FriendRequestsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: FriendRequests
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            var projectContext = _context.FriendRequests.Include(f => f.Receiver).Include(f => f.Sender).ToList();
            if (searchString != null)
            {
                projectContext = projectContext.Where(a => a.Sender.Fname.ToLower().Contains(searchString)).ToList();
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
            var pagedProducts = new StaticPagedList<FriendRequest>(
                projectContext.Skip((pageNumber - 1) * pageSize).Take(pageSize),
                pageNumber,
                pageSize,
                totalItems);
            ViewBag.CurrentFilter = searchString;
            return View(pagedProducts);
        }

        // GET: FriendRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FriendRequests == null)
            {
                return NotFound();
            }

            var friendRequest = await _context.FriendRequests
                .Include(f => f.Receiver)
                .Include(f => f.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (friendRequest == null)
            {
                return NotFound();
            }

            return View(friendRequest);
        }

        // GET: FriendRequests/Create
        public IActionResult Create()
        {
            ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Fname");
            ViewData["SenderId"] = new SelectList(_context.Users, "Id", "Fname");
            return View();
        }

        // POST: FriendRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SenderId,ReceiverId,Status,SentDate")] FriendRequest friendRequest)
        {
            if (ModelState.IsValid)
            {
                friendRequest.SentDate = DateTime.Now;  
                _context.Add(friendRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Fname", friendRequest.ReceiverId);
            ViewData["SenderId"] = new SelectList(_context.Users, "Id", "Fname", friendRequest.SenderId);
            return View(friendRequest);
        }

        // GET: FriendRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FriendRequests == null)
            {
                return NotFound();
            }

            var friendRequest = await _context.FriendRequests.FindAsync(id);
            if (friendRequest == null)
            {
                return NotFound();
            }
            ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Fname", friendRequest.ReceiverId);
            ViewData["SenderId"] = new SelectList(_context.Users, "Id", "Fname", friendRequest.SenderId);
            return View(friendRequest);
        }

        // POST: FriendRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SenderId,ReceiverId,Status")] FriendRequest friendRequest)
        {
            if (id != friendRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingFriend = _context.FriendRequests.Find(id);
                existingFriend.SenderId = friendRequest.SenderId;
                existingFriend.ReceiverId = friendRequest.ReceiverId;
                existingFriend.Status = friendRequest.Status;
                try
                {
                    _context.Update(existingFriend);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FriendRequestExists(friendRequest.Id))
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
            ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Id", friendRequest.ReceiverId);
            ViewData["SenderId"] = new SelectList(_context.Users, "Id", "Id", friendRequest.SenderId);
            return View(friendRequest);
        }

        // GET: FriendRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FriendRequests == null)
            {
                return NotFound();
            }

            var friendRequest = await _context.FriendRequests
                .Include(f => f.Receiver)
                .Include(f => f.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (friendRequest == null)
            {
                return NotFound();
            }

            return View(friendRequest);
        }

        // POST: FriendRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FriendRequests == null)
            {
                return Problem("Entity set 'ProjectContext.FriendRequests'  is null.");
            }
            var friendRequest = await _context.FriendRequests.FindAsync(id);
            if (friendRequest != null)
            {
                _context.FriendRequests.Remove(friendRequest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FriendRequestExists(int id)
        {
          return _context.FriendRequests.Any(e => e.Id == id);
        }
    }
}
