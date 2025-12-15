using api_pd.Data;
using api_pd.DTOs.Product;
using api_pd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_pd.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // READ ALL
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            return Ok(products);
        }

        // READ BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();
            return Ok(product);
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDto dto)
        {
            bool categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == dto.CategoryId);

            if (!categoryExists)
                return BadRequest("Category ไม่ถูกต้อง");

            bool productExists = await _context.Products
                .AnyAsync(p => p.Name == dto.Name && p.CategoryId == dto.CategoryId);

            if (productExists)
                return BadRequest("มีสินค้านี้ในหมวดแล้ว");

            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }


        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID ไม่ตรงกัน");

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            bool exists = await _context.Products.AnyAsync(p =>
                p.Name == dto.Name &&
                p.CategoryId == dto.CategoryId &&
                p.Id != id);

            if (exists)
                return BadRequest("ชื่อสินค้าซ้ำ");

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();
            return Ok(product);
        }


        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
