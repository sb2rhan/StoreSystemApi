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
    public class PurchaseProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PurchaseProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<IEnumerable<PurchaseProduct>>> GetPurchaseProduct()
        {
            if (_context.PurchaseProduct == null)
            {
                return NotFound();
            }
            return await _context.PurchaseProduct.ToListAsync();
        }

        [HttpGet("{purchase_id}/{product_id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<PurchaseProduct>> GetPurchaseProduct(string purchase_id, string product_id)
        {
            if (_context.PurchaseProduct == null)
            {
                return NotFound();
            }
            var purchaseProduct = await _context.PurchaseProduct.FindAsync(new Guid(purchase_id), new Guid(product_id));

            if (purchaseProduct == null)
            {
                return NotFound();
            }

            return purchaseProduct;
        }

        [HttpPut("{purchase_id}/{product_id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> PutPurchaseProduct(string purchase_id, string product_id, PurchaseProduct purchaseProduct)
        {
            Guid purchase_guid = new Guid(purchase_id);
            Guid product_guid = new Guid(product_id);

            if (purchase_guid != purchaseProduct.PurchaseId || product_guid != purchaseProduct.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(purchaseProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseProductExists(purchase_guid, product_guid))
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
        [Authorize(Roles = $"{UserRoles.Cashier},{UserRoles.Admin}")]
        public async Task<ActionResult<PurchaseProduct>> PostPurchaseProduct(PurchaseProduct purchaseProduct)
        {
            if (_context.PurchaseProduct == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PurchaseProduct'  is null.");
            }
            _context.PurchaseProduct.Add(purchaseProduct);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PurchaseProductExists(purchaseProduct.PurchaseId, purchaseProduct.ProductId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPurchaseProduct", 
                new { PurchaseId = purchaseProduct.PurchaseId, ProductId = purchaseProduct.ProductId },
                purchaseProduct);
        }

        [HttpDelete("{purchase_id}/{product_id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeletePurchaseProduct(string purchase_id, string product_id)
        {
            if (_context.PurchaseProduct == null)
            {
                return NotFound();
            }
            var purchaseProduct = await _context.PurchaseProduct.FindAsync(new Guid(purchase_id), new Guid(product_id));
            if (purchaseProduct == null)
            {
                return NotFound();
            }

            _context.PurchaseProduct.Remove(purchaseProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PurchaseProductExists(Guid purchase_id, Guid product_id)
        {
            return (
                _context.PurchaseProduct?
                .Any(e => e.PurchaseId == purchase_id && e.ProductId == product_id)
            ).GetValueOrDefault();
        }
    }
}
