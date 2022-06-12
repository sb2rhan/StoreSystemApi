using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreSystemApi.Models;
using StoreSystemApi.Models.Auth;
using StoreSystemApi.Models.Entities;

namespace StoreSystemApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [EnableCors]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRoles.Cashier},{UserRoles.Admin}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            return await _context.Products.Where(p => p.DeletedAt == null).ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{UserRoles.Cashier},{UserRoles.Admin}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(new Guid(id));

            if (product == null || product.DeletedAt != null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpGet("barcode/{barcode}")]
        [Authorize(Roles = $"{UserRoles.Cashier},{UserRoles.Admin}")]
        public async Task<ActionResult<Product>> GetProductByBarcode(string barcode)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Barcode == barcode);

            if (product == null || product.DeletedAt != null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{UserRoles.Cashier},{UserRoles.Admin}")]
        public async Task<IActionResult> PutProduct(string id, Product product)
        {
            Guid guid = new Guid(id);
            if (guid != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(guid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(new Guid(id));
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(Guid id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
