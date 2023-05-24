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
    public class UserAwardBalancesController : Controller
    {
        private readonly ProjectContext _context;

        public UserAwardBalancesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: UserAwardBalances
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            var projectContext = _context.UserAwardBalances.Include(u => u.Award).ToList();
            if (searchString != null)
            {
                projectContext = projectContext.Where(a => a.Award.Name.ToLower().Contains(searchString)).ToList();
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

            var pagedProducts = new StaticPagedList<UserAwardBalance>(
                projectContext.Skip((pageNumber - 1) * pageSize).Take(pageSize),
                pageNumber,
                pageSize,
                totalItems);
            ViewBag.CurrentFilter = searchString;
            return View(pagedProducts);
        }

        // GET: UserAwardBalances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserAwardBalances == null)
            {
                return NotFound();
            }

            var userAwardBalance = await _context.UserAwardBalances
                .Include(u => u.Award)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userAwardBalance == null)
            {
                return NotFound();
            }

            return View(userAwardBalance);
        }

        // GET: UserAwardBalances/Create
        public IActionResult Create()
        {
            ViewData["AwardId"] = new SelectList(_context.Awards, "Id", "Name");
            return View();
        }

        // POST: UserAwardBalances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AwardId,Balance")] UserAwardBalance userAwardBalance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userAwardBalance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AwardId"] = new SelectList(_context.Awards, "Id", "Name", userAwardBalance.AwardId);
            return View(userAwardBalance);
        }

        // GET: UserAwardBalances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserAwardBalances == null)
            {
                return NotFound();
            }

            var userAwardBalance = await _context.UserAwardBalances.FindAsync(id);
            if (userAwardBalance == null)
            {
                return NotFound();
            }
            ViewData["AwardId"] = new SelectList(_context.Awards, "Id", "Name", userAwardBalance.AwardId);
            return View(userAwardBalance);
        }

        // POST: UserAwardBalances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AwardId,Balance")] UserAwardBalance userAwardBalance)
        {
            if (id != userAwardBalance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAwardBalance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAwardBalanceExists(userAwardBalance.Id))
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
            ViewData["AwardId"] = new SelectList(_context.Awards, "Id", "Name", userAwardBalance.AwardId);
            return View(userAwardBalance);
        }

        // GET: UserAwardBalances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserAwardBalances == null)
            {
                return NotFound();
            }

            var userAwardBalance = await _context.UserAwardBalances
                .Include(u => u.Award)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userAwardBalance == null)
            {
                return NotFound();
            }

            return View(userAwardBalance);
        }

        // POST: UserAwardBalances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserAwardBalances == null)
            {
                return Problem("Entity set 'ProjectContext.UserAwardBalances'  is null.");
            }
            var userAwardBalance = await _context.UserAwardBalances.FindAsync(id);
            if (userAwardBalance != null)
            {
                _context.UserAwardBalances.Remove(userAwardBalance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserAwardBalanceExists(int id)
        {
          return _context.UserAwardBalances.Any(e => e.Id == id);
        }
    }
}
