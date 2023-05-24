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
    public class UserAwardPurchasesController : Controller
    {
        private readonly ProjectContext _context;

        public UserAwardPurchasesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: UserAwardPurchases
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            var projectContext = _context.UserAwardPurchases.Include(u => u.Award).Include(u => u.User).ToList();
            if (searchString != null)
            {
                projectContext = projectContext.Where(a => a.User.Fname.ToLower().Contains(searchString)||a.Award.Name.ToLower().Contains(searchString)).ToList();
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

            var pagedProducts = new StaticPagedList<UserAwardPurchase>(
                projectContext.Skip((pageNumber - 1) * pageSize).Take(pageSize),
                pageNumber,
                pageSize,
                totalItems);
            ViewBag.CurrentFilter = searchString;
            return View(pagedProducts);
        }

        // GET: UserAwardPurchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserAwardPurchases == null)
            {
                return NotFound();
            }

            var userAwardPurchase = await _context.UserAwardPurchases
                .Include(u => u.Award)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userAwardPurchase == null)
            {
                return NotFound();
            }

            return View(userAwardPurchase);
        }

        // GET: UserAwardPurchases/Create
        public IActionResult Create()
        {
            ViewData["AwardId"] = new SelectList(_context.Awards, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname");
            return View();
        }

        // POST: UserAwardPurchases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AwardId,Quantity,TotalAmount,UserId")] UserAwardPurchase userAwardPurchase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userAwardPurchase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AwardId"] = new SelectList(_context.Awards, "Id", "Name", userAwardPurchase.AwardId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname", userAwardPurchase.UserId);
            return View(userAwardPurchase);
        }

        // GET: UserAwardPurchases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserAwardPurchases == null)
            {
                return NotFound();
            }

            var userAwardPurchase = await _context.UserAwardPurchases.FindAsync(id);
            if (userAwardPurchase == null)
            {
                return NotFound();
            }
            ViewData["AwardId"] = new SelectList(_context.Awards, "Id", "Name", userAwardPurchase.AwardId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname", userAwardPurchase.UserId);
            return View(userAwardPurchase);
        }

        // POST: UserAwardPurchases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AwardId,Quantity,TotalAmount,UserId")] UserAwardPurchase userAwardPurchase)
        {
            if (id != userAwardPurchase.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAwardPurchase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAwardPurchaseExists(userAwardPurchase.Id))
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
            ViewData["AwardId"] = new SelectList(_context.Awards, "Id", "Name", userAwardPurchase.AwardId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname", userAwardPurchase.UserId);
            return View(userAwardPurchase);
        }

        // GET: UserAwardPurchases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserAwardPurchases == null)
            {
                return NotFound();
            }

            var userAwardPurchase = await _context.UserAwardPurchases
                .Include(u => u.Award)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userAwardPurchase == null)
            {
                return NotFound();
            }

            return View(userAwardPurchase);
        }

        // POST: UserAwardPurchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserAwardPurchases == null)
            {
                return Problem("Entity set 'ProjectContext.UserAwardPurchases'  is null.");
            }
            var userAwardPurchase = await _context.UserAwardPurchases.FindAsync(id);
            if (userAwardPurchase != null)
            {
                _context.UserAwardPurchases.Remove(userAwardPurchase);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserAwardPurchaseExists(int id)
        {
          return _context.UserAwardPurchases.Any(e => e.Id == id);
        }
    }
}
