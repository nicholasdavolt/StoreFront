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
    public class ProductsController : Controller
    {
        private readonly GuitarShopContext _context;

        public ProductsController(GuitarShopContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var guitarShopContext = _context.Products.Where(p => p.Status.Status1 != "Display-Only" && p.UnitsInStock != 0).Include(p => p.Builder).Include(p => p.Status).Include(p => p.Type);
            return View(await guitarShopContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Builder)
                .Include(p => p.Status)
                .Include(p => p.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["BuilderId"] = new SelectList(_context.Builders, "Id", "Builder1");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id");
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Type1");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductName,ProductDesc,UnitsInStock,SellPrice,PurchasePrice,TypeId,StatusId,BuilderId,ProductImage")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuilderId"] = new SelectList(_context.Builders, "Id", "Builder1", product.BuilderId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", product.StatusId);
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Type1", product.TypeId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BuilderId"] = new SelectList(_context.Builders, "Id", "Builder1", product.BuilderId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", product.StatusId);
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Type1", product.TypeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductName,ProductDesc,UnitsInStock,SellPrice,PurchasePrice,TypeId,StatusId,BuilderId,ProductImage")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["BuilderId"] = new SelectList(_context.Builders, "Id", "Builder1", product.BuilderId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", product.StatusId);
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Type1", product.TypeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Builder)
                .Include(p => p.Status)
                .Include(p => p.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'GuitarShopContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
