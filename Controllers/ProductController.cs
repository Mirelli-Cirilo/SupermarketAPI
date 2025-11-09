using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.DTOs;
using SupermarketAPI.Models;

namespace SupermarketAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Inner")]
        public IActionResult Create([FromBody] CreateProductDto productDto)
        {
            if (productDto == null)
                return BadRequest("Invalid product.");

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Category = productDto.Category,
                Stock = productDto.Stock, 
                RegistrationDate = DateTime.UtcNow
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            var response = new ResponseProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Category = product.Category.ToString(),
                Stock = product.Stock,
                RegistrationDate = product.RegistrationDate
            };

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, response);
        }

        [HttpGet]
        [Authorize(Roles = "Customer, Inner, Admin")]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Invalid pagination parameters.");

            var total = _context.Products.Count();
            var products = _context.Products
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ResponseProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Category = x.Category.ToString(),
                    Stock = x.Stock,
                    RegistrationDate = x.RegistrationDate
                })
                .ToList();

            return Ok(new
            {
                page,
                pageSize,
                totalItems = total,
                items = products
            });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Customer, Inner, Admin")]
        public IActionResult GetById(Guid id)
        { 
            var product = _context.Products.Find(id);

            if (product == null) 
                return NotFound("Product not found"); 

            var response = new ResponseProductDto { 
                Id = product.Id, Name = product.Name,
                Price = product.Price, 
                Category = product.Category.ToString(), 
                Stock = product.Stock, 
                RegistrationDate = product.RegistrationDate };

            return Ok(response); 
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(Guid id, [FromBody] CreateProductDto productDto)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound("Product not found");

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Category = productDto.Category;
            product.Stock = productDto.Stock;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound("Product not found");

            _context.Products.Remove(product);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
