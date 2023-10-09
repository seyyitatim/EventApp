using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventAppMVC.Contexts;
using EventAppMVC.Entities;
using EventAppMVC.Models;

namespace EventAppMVC.Controllers
{
    public class EventsController : Controller
    {
        private readonly MySqlDbContext _context;
        private readonly IWebHostEnvironment hostEnvironment;

        public EventsController(MySqlDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return _context.Events != null ?
                        View(await _context.Events.ToListAsync()) :
                        Problem("Entity set 'MySqlDbContext.Events'  is null.");
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Place,Time,IsFree,Price,Description,Image")] CreateEventModel model)
        {
            if (ModelState.IsValid)
            {
                Event entity = new()
                {
                    Title = model.Title,
                    Place = model.Place,
                    Description = model.Description,
                    Price = model.Price,
                    Time = model.Time,
                    IsFree = model.IsFree,
                    Image = ""
                };

                entity.Image = AddImageToFolder(model.Image);

                _context.Events.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private string AddImageToFolder(IFormFile image)
        {
            string wwwRootPath = hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(image.FileName);
            string extension = Path.GetExtension(image.FileName);
            string imageName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath + "/image/", imageName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            return imageName;
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Place,Time,IsFree,Price,Description,Image")] UpdateEventModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var entity = await _context.Events.FindAsync(id);

                    entity.Title = model.Title;
                    entity.Description = model.Description;
                    entity.Price = model.Price;
                    entity.Time = model.Time;
                    entity.IsFree = model.IsFree;
                    entity.Place = model.Place;

                    if (model.Image.Length > 0)
                    {
                        DeleteImage(entity.Image);
                        entity.Image = AddImageToFolder(model.Image);
                    }

                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(model.Id))
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
            return View(model);
        }

        private void DeleteImage(string path)
        {
            string wwwRootPath = hostEnvironment.WebRootPath;
            string imagePath = System.IO.Path.Combine(wwwRootPath + "/image/", path);

            FileInfo file = new FileInfo(imagePath);
            if (file.Exists)
            {
                file.Delete();
            }
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'MySqlDbContext.Events'  is null.");
            }
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                DeleteImage(@event.Image);
                _context.Events.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
