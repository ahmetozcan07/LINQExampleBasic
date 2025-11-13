using LinqExampleApp.Data;
using LinqExampleApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LinqExampleApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        //public IActionResult Index()
        //{
        //    var Products = from p in _context.Products
        //                   orderby p.Price descending
        //                   select p;
        //    return View(Products.ToList());
        //}

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult Filtered(decimal? minPrice, string? sortOrder, string? category)
        {
            // All products can be seen at first
            var query = _context.Products.AsQueryable();
            //var query = from p in _context.Products
            //            select p;

            // LINQ query according to user input
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price > minPrice.Value);
            }
            // desc or asc
            if (sortOrder == "desc")
                query = query.OrderByDescending(p => p.Price);
            else
                query = query.OrderBy(p => p.Price);

            // filter category
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }


            // Creating category list 
            var categories = _context.Products
                            .Select(p => p.Category)
                            .Distinct()
                            .OrderBy(c => c)
                            .ToList();

            // sending category list to view with ViewBag
            ViewBag.Categories = categories;


            var products = query.ToList();
            return View(products);
        }
    }
}
