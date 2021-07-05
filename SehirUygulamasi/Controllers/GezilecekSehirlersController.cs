using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SehirUygulamasi.Data;
using SehirUygulamasi.Models;

namespace SehirUygulamasi.Controllers
{
    [Authorize]
    public class GezilecekSehirlersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<CetUser> _userManager;

        public GezilecekSehirlersController(ApplicationDbContext context, UserManager<CetUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
       
        // GET: GezilecekSehirlers
        public async Task<IActionResult> Index(SearchViewModel searchModel)
        {
            var cetUser = await _userManager.GetUserAsync(HttpContext.User);

            var query = _context.GezilecekSehirlers.Include(g => g.Category).Where(g=>g.CetUserId==cetUser.Id);
            if (!searchModel.ShowAll)
            {
                query = query.Where(g => !g.IsCompleted);
            }
            if (!String.IsNullOrWhiteSpace(searchModel.SearchText))
            {
                query = query.Where(g => g.Title.Contains(searchModel.SearchText)); 
            }

            query = query.OrderBy(g=>g.DueDate);
            searchModel.Result = await query.ToListAsync();

            return View(searchModel);
        }

        // GET: GezilecekSehirlers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gezilecekSehirler = await _context.GezilecekSehirlers
                .Include(g => g.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gezilecekSehirler == null)
            {
                return NotFound();
            }

            return View(gezilecekSehirler);
        }

        // GET: GezilecekSehirlers/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.CategorySelectList = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: GezilecekSehirlers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,IsCompleted,DueDate,CategoryId")] GezilecekSehirler gezilecekSehirler)
        {
            var cetUser = await _userManager.GetUserAsync(HttpContext.User);
            gezilecekSehirler.CetUserId = cetUser.Id;
            if (ModelState.IsValid)
            {
                _context.Add(gezilecekSehirler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", gezilecekSehirler.CategoryId);
            return View(gezilecekSehirler);
        }

        // GET: GezilecekSehirlers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gezilecekSehirler = await _context.GezilecekSehirlers.FindAsync(id);

            
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (gezilecekSehirler.CetUserId != currentUser.Id)
            {
                return Unauthorized();
            }
            if (gezilecekSehirler == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", gezilecekSehirler.CategoryId);
            return View(gezilecekSehirler);
        }

        // POST: GezilecekSehirlers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsCompleted,DueDate,CategoryId,CreatedDate,CetUserId")] GezilecekSehirler gezilecekSehirler)
        {
            if (id != gezilecekSehirler.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldTodo = await _context.GezilecekSehirlers.FindAsync(id);
                    var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                    if (oldTodo.CetUserId != currentUser.Id)
                    {
                        return Unauthorized();
                    }
                    oldTodo.Title = gezilecekSehirler.Title;
                    oldTodo.CompletedDate = gezilecekSehirler.CompletedDate;
                    oldTodo.CategoryId = gezilecekSehirler.CategoryId;
                    oldTodo.IsCompleted = gezilecekSehirler.IsCompleted;
                    oldTodo.Description = gezilecekSehirler.Description;
                    oldTodo.DueDate = gezilecekSehirler.DueDate;

                    _context.Update(oldTodo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GezilecekSehirlerExists(gezilecekSehirler.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", gezilecekSehirler.CategoryId);
            return View(gezilecekSehirler);
        }

        // GET: GezilecekSehirlers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gezilecekSehirler = await _context.GezilecekSehirlers
                .Include(g => g.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gezilecekSehirler == null)
            {
                return NotFound();
            }

            return View(gezilecekSehirler);
        }

        // POST: GezilecekSehirlers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gezilecekSehirler = await _context.GezilecekSehirlers.FindAsync(id);
            _context.GezilecekSehirlers.Remove(gezilecekSehirler);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> MakeComplete(int id, bool showAll)
        {
            return await ChangeStatus(id, true, showAll);

        }

        public async Task<IActionResult> MakeInComplete(int id, bool showAll)
        {
            return await ChangeStatus(id, false, showAll);

        }

        private async Task<IActionResult> ChangeStatus(int id, bool status, bool currentShowallValue)
        {
            var sehirItemItem = _context.GezilecekSehirlers.FirstOrDefault(x => x.Id == id);
            if (sehirItemItem == null)
            {
                return NotFound();
            }
            sehirItemItem.IsCompleted = status;
            sehirItemItem.CompletedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { showall = currentShowallValue });
        }


        private bool GezilecekSehirlerExists(int id)
        {
            return _context.GezilecekSehirlers.Any(e => e.Id == id);
        }
    }
}
