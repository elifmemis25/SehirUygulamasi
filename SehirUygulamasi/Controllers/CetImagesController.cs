using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SehirUygulamasi.Data;
using SehirUygulamasi.Models;

namespace SehirUygulamasi.Controllers
{
    public class CetImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment hostEnvironment;

        public CetImagesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;
        }

        // GET: CetImages
        public async Task<IActionResult> Index()
        {
            return View(await _context.CetImages.ToListAsync());
        }

        // GET: CetImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cetImage = await _context.CetImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cetImage == null)
            {
                return NotFound();
            }

            return View(cetImage);
        }

        // GET: CetImages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CetImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ImageFile")] CetImage cetImage)
        {
            if (ModelState.IsValid)
            {
                string root = hostEnvironment.WebRootPath;
                //cetImage.ImageName = cetImage.ImageFile.FileName;
                var extension = Path.GetExtension(cetImage.ImageFile.FileName);
                var fileName = Guid.NewGuid().ToString() + extension;
                cetImage.ImageName = fileName;
                var path = root+ "\\upload\\ " + fileName;
                using(var stream=new FileStream(path, FileMode.Create))
                {
                    await cetImage.ImageFile.CopyToAsync(stream);
                }


                _context.Add(cetImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cetImage);
        }

        // GET: CetImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cetImage = await _context.CetImages.FindAsync(id);
            if (cetImage == null)
            {
                return NotFound();
            }
            return View(cetImage);
        }

        // POST: CetImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ImageName")] CetImage cetImage)
        {
            if (id != cetImage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cetImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CetImageExists(cetImage.Id))
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
            return View(cetImage);
        }

        // GET: CetImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cetImage = await _context.CetImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cetImage == null)
            {
                return NotFound();
            }

            return View(cetImage);
        }

        // POST: CetImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cetImage = await _context.CetImages.FindAsync(id);
            _context.CetImages.Remove(cetImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CetImageExists(int id)
        {
            return _context.CetImages.Any(e => e.Id == id);
        }
    }
}
