using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.DATA.EF.Models;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using StoreFront.UI.MVC.Utilities;
using X.PagedList;

namespace StoreFront.UI.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly GuitarShopContext _context;

        //This field stores server folder structure info to facillitate image saving.
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(GuitarShopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        // GET: Products
        
        public async Task<IActionResult> Index()
        {
            var guitarShopContext = _context.Products.Include(p => p.Builder).Include(p => p.Status).Include(p => p.Type).Include(p => p.OrderProducts);
            return View(await guitarShopContext.ToListAsync());
        }
        [AllowAnonymous]
        public async Task<IActionResult> TiledProducts (string searchTerm, int page = 1)
        {
            //Variable for page size
            int pageSize = 5;
            var products = _context.Products.Where(p => p.Status.Status1 != "Display-Only" && p.UnitsInStock != 0).Include(p => p.Builder).Include(p => p.Status).Include(p => p.Type).ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                ViewBag.SearchTerm = searchTerm;
                searchTerm = searchTerm.ToLower();
                products = products.Where(p => p.ProductName.ToLower().Contains(searchTerm) || p.Builder.Builder1.ToLower().Contains(searchTerm) || p.ProductDesc.ToLower().Contains(searchTerm)).ToList();

                ViewBag.NbrResults = products.Count;
            }
            else
            {
                ViewBag.NbrResults = null;
                ViewBag.SearchTerm = null;
            }

            return View(products.ToPagedList(page, pageSize));
        }
       
        // GET: Products/Details/5
        [AllowAnonymous]
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
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Status1");
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Type1");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductName,ProductDesc,UnitsInStock,SellPrice,PurchasePrice,TypeId,StatusId,BuilderId,ProductImage,Image")] Product product)
        {
            if (ModelState.IsValid)
            {
                //Check to see if an Image was uploaded

                if (product.Image != null)
                {
                    //Retrive the uploaded file extension
                    string ext = Path.GetExtension(product.Image.FileName);

                    //Valid image extensions for upload verification
                    string[] validExts = { ".jpg", ".jpeg", ".gif", ".png" };

                    //Verify that the uploaded file has an approaved extension and meets size criteria
                    //TODO: Maybe move this into a method that can be shared with the edit image upload.

                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)
                    {
                        //Create GUID file name
                        product.ProductImage = Guid.NewGuid() + ext;

                        //create holder for the Web Root path

                        string webRootPath = _webHostEnvironment.WebRootPath;

                        //use the above to create the full path for image storage
                        string fullImagePath = webRootPath + "/img/";

                        //open a MemoryStream to read the image into memory
                        using (var memoryStream = new MemoryStream())
                        {
                            //Transfer the file to Memory
                            await product.Image.CopyToAsync(memoryStream);

                            using (var img = Image.FromStream(memoryStream))
                            {
                                int maxImageSize = 500;
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(fullImagePath, product.ProductImage, img, maxImageSize, maxThumbSize);
                            }


                        }
                    }

                }
                else
                {
                    product.ProductImage = "noimage.png";
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuilderId"] = new SelectList(_context.Builders, "Id", "Builder1", product.BuilderId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Status1", product.StatusId);
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
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Status1", product.StatusId);
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Type1", product.TypeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductName,ProductDesc,UnitsInStock,SellPrice,PurchasePrice,TypeId,StatusId,BuilderId,ProductImage,Image")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {


                //Retain the existing image file name for future deletion
                string oldImageName = product.ProductImage;

                if (product.Image != null)
                {
                    

                    string ext = Path.GetExtension(product.Image.FileName);

                    long imgLength = product.Image.Length;

                    //Verify image extension and length, if valid proceed with upload
                    if (ImageVerify(ext, imgLength))
                    {
                        //Generate a unique file name

                        product.ProductImage = Guid.NewGuid() + ext;

                        //Build the full path for image upload
                        string webRootPath = _webHostEnvironment.WebRootPath;

                        string fullPath = webRootPath + "/img/";

                        //If necessary, delete the old image

                        if (oldImageName != "noimage.png")
                        {
                            ImageUtility.Delete(fullPath, oldImageName);
                        }

                        //Save the new image to the server

                        using (var memoryStream = new MemoryStream())
                        {
                            await product.Image.CopyToAsync(memoryStream);

                            using (var img = Image.FromStream(memoryStream))
                            {
                                int maxImageSize = 260;
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(fullPath, product.ProductImage, img, maxImageSize, maxThumbSize);
                            }
                        }
                    }
                }

                
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
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Status1", product.StatusId);
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

        public bool ImageVerify(string ext, long imgLength)
        {
            //Valid image extensions for upload verification
            string[] validExts = { ".jpg", ".jpeg", ".gif", ".png" };

            if (validExts.Contains(ext.ToLower()) && imgLength < 4_194_303)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
