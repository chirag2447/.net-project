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
    public class CommunityMembersController : Controller
    {
        private readonly ProjectContext _context;

        public CommunityMembersController(ProjectContext context)
        {
            _context = context;
        }

        // GET: CommunityMembers
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            var projectContext = _context.CommunityMembers.Include(c => c.Community).Include(c => c.User).ToList();
            if (searchString != null)
            {
                projectContext = projectContext.Where(a => a.User.Fname.ToLower().Contains(searchString) || a.Community.Name.ToLower().Contains(searchString)).ToList();
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
            
            if(totalItems%pageSize == 0)
            {

            ViewBag.totalPage = b;

            }
            else
            {

            ViewBag.totalPage = b+1;
            }
            /* a = Math.Ceiling(b) + 1;*/
            

            

            var pagedProducts = new StaticPagedList<CommunityMember>(
                projectContext.Skip((pageNumber - 1) * pageSize).Take(pageSize),
                pageNumber,
                pageSize,
                totalItems);
            ViewBag.CurrentFilter = searchString;
            return View(pagedProducts);
        }

        // GET: CommunityMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CommunityMembers == null)
            {
                return NotFound();
            }

            var communityMember = await _context.CommunityMembers
                .Include(c => c.Community)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (communityMember == null)
            {
                return NotFound();
            }

            return View(communityMember);
        }

        // GET: CommunityMembers/Create
        public IActionResult Create()
        {
            ViewData["CommunityId"] = new SelectList(_context.Communities, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname");
            return View();
        }

        // POST: CommunityMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,CommunityId")] CommunityMember communityMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(communityMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CommunityId"] = new SelectList(_context.Communities, "Id", "Name", communityMember.CommunityId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname", communityMember.UserId);

            return View(communityMember);
        }

        // GET: CommunityMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CommunityMembers == null)
            {
                return NotFound();
            }

            var communityMember = await _context.CommunityMembers.FindAsync(id);
            if (communityMember == null)
            {
                return NotFound();
            }
            ViewData["CommunityId"] = new SelectList(_context.Communities, "Id", "Name", communityMember.CommunityId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fname", communityMember.UserId);
            return View(communityMember);
        }

        // POST: CommunityMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,CommunityId")] CommunityMember communityMember)
        {
            if (id != communityMember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(communityMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommunityMemberExists(communityMember.Id))
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
            ViewData["CommunityId"] = new SelectList(_context.Communities, "Id", "Id", communityMember.CommunityId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", communityMember.UserId);
            return View(communityMember);
        }

        // GET: CommunityMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CommunityMembers == null)
            {
                return NotFound();
            }

            var communityMember = await _context.CommunityMembers
                .Include(c => c.Community)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (communityMember == null)
            {
                return NotFound();
            }

            return View(communityMember);
        }

        // POST: CommunityMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CommunityMembers == null)
            {
                return Problem("Entity set 'ProjectContext.CommunityMembers'  is null.");
            }
            var communityMember = await _context.CommunityMembers.FindAsync(id);
            if (communityMember != null)
            {
                _context.CommunityMembers.Remove(communityMember);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommunityMemberExists(int id)
        {
          return _context.CommunityMembers.Any(e => e.Id == id);
        }
    }
}
