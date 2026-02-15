using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeautyStandard.Data;
using BeautyStandard.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeautyStandard.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Category != null ? p.Category.Name : "")
                .ThenBy(p => p.Name)
                .ToListAsync();
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
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
            // Получаем список категорий из базы данных
            var categories = _context.Categories.OrderBy(c => c.Name).ToList();

            // Передаём список в представление через ViewBag
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            // Проверка на дубликат по названию в категории
            bool exists = await _context.Products
                .AnyAsync(p => p.Name == product.Name && p.CategoryId == product.CategoryId);

            if (exists)
            {
                ModelState.AddModelError("Name", "Товар с таким названием уже существует в этой категории");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Товар успешно добавлен";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при сохранении: " + ex.Message);
                }
            }

            ViewBag.Categories = _context.Categories.OrderBy(c => c.Name).ToList();
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            // Получаем список категорий
            var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            // Последний параметр (product.CategoryId) — выбранная категория текущего товара

            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            // Проверка на дубликат (исключая текущий товар)
            bool exists = await _context.Products
                .AnyAsync(p => p.Name == product.Name && p.CategoryId == product.CategoryId && p.Id != product.Id);

            if (exists)
            {
                ModelState.AddModelError("Name", "Товар с таким названием уже существует в этой категории");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Товар успешно обновлен";
                    return RedirectToAction(nameof(Index));
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
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при обновлении: " + ex.Message);
                }
            }

            ViewBag.Categories = _context.Categories.OrderBy(c => c.Name).ToList();
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
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
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Товар успешно удален";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}