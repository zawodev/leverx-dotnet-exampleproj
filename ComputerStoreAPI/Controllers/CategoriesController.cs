﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Models;
using ComputerStore.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;

namespace ComputerStoreAPI.Controllers {
    [Authorize]
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase {
        private readonly StoreContext _context;
        public CategoriesController(StoreContext context) => _context = context;

        /// <summary>
        /// get all categories
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll() {
            return Ok(await _context.Categories.ToListAsync());
        }

        /// <summary>
        /// get category by id
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(int id) {
            var cat = await _context.Categories.FindAsync(id);
            if (cat == null) return NotFound();
            return Ok(cat);
        }

        /// <summary>
        /// create new category
        /// </summary>
        [Authorize(Roles = "Manager,Admin")]
        [HttpPost]
        public async Task<ActionResult<Category>> Create(Category category) {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        /// <summary>
        /// update existing category
        /// </summary>
        [Authorize(Roles = "Manager,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Category category) {
            if (id != category.Id) return BadRequest();
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// delete category by id
        /// </summary>
        [Authorize(Roles = "Manager,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var cat = await _context.Categories.FindAsync(id);
            if (cat == null) return NotFound();
            _context.Categories.Remove(cat);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
