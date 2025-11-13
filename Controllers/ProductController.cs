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
        public IActionResult Filtered(decimal? minPrice, string? sortOrder)
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

            var products = query.ToList();
            return View(products);
        }
    }
}
