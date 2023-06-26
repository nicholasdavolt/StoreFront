using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.DATA.EF.Models;

namespace StoreFront.UI.MVC.Controllers
{
    public class BuildersController : Controller
    {
        private readonly GuitarShopContext _context;

        public BuildersController(GuitarShopContext context)
        {
            _context = context;
        }

        // GET: Builders
        public async Task<IActionResult> Index()
        {
              return _context.Builders != null ? 
                          View(await _context.Builders.ToListAsync()) :
                          Problem("Entity set 'GuitarShopContext.Builders'  is null.");
        }

        // GET: Builders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Builders == null)
            {
                return NotFound();
            }

            var builder = await _context.Builders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (builder == null)
            {
                return NotFound();
            }

            return View(builder);
        }

        // GET: Builders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Builders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Builder1")] Builder builder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(builder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(builder);
        }

        // GET: Builders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Builders == null)
            {
                return NotFound();
            }

            var builder = await _context.Builders.FindAsync(id);
            if (builder == null)
            {
                return NotFound();
            }
            return View(builder);
        }

        // POST: Builders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Builder1")] Builder builder)
        {
            if (id != builder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(builder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuilderExists(builder.Id))
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
            return View(builder);
        }

        //AJAX based deletion
        [AcceptVerbs("POST")]
        
        public JsonResult AjaxDelete(int id)
        {
            Builder builder = _context.Builders.Find(id);

            _context.Builders.Remove(builder);

            _context.SaveChanges();

            string confirmMessage = $"Deleted {builder.Builder1} from the database!";

            return Json(new { id = id, message = confirmMessage });

        }

        //// GET: Builders/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Builders == null)
        //    {
        //        return NotFound();
        //    }

        //    var builder = await _context.Builders
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (builder == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(builder);
        //}

        //// POST: Builders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Builders == null)
        //    {
        //        return Problem("Entity set 'GuitarShopContext.Builders'  is null.");
        //    }
        //    var builder = await _context.Builders.FindAsync(id);
        //    if (builder != null)
        //    {
        //        _context.Builders.Remove(builder);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool BuilderExists(int id)
        {
          return (_context.Builders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
