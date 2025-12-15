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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category!.Name
                })
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Id == id)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category!.Name
                })
                .FirstOrDefaultAsync();

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // ===================== POST =====================
        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> Create(ProductCreateDto dto)
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

            var response = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                CategoryName = (await _context.Categories
                    .Where(c => c.Id == product.CategoryId)
                    .Select(c => c.Name)
                    .FirstAsync())
            };

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, response);
        }

        // ===================== PUT =====================

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID ไม่ตรงกัน");

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            bool categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == dto.CategoryId);

            if (!categoryExists)
                return BadRequest("Category ID ไม่ถูกต้อง"); 
                                                             

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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "เกิดข้อผิดพลาดในการบันทึกข้อมูล: " + ex.Message);
            }

            return NoContent();
        }

        // ===================== DELETE =====================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
