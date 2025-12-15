using api_pd.Data;
using api_pd.DTOs.Category;
using api_pd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_pd.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _context.Categories.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto dto)
        {
            bool exists = await _context.Categories
                .AnyAsync(c => c.Name == dto.Name);

            if (exists)
                return BadRequest("มีหมวดนี้อยู่แล้ว");

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID ไม่ตรงกัน");

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            bool exists = await _context.Categories
                .AnyAsync(c => c.Name == dto.Name && c.Id != id);

            if (exists)
                return BadRequest("ชื่อหมวดซ้ำ");

            category.Name = dto.Name;
            category.Description = dto.Description;

            await _context.SaveChangesAsync();
            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
