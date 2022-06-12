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
    public class BonusCardsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BonusCardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<IEnumerable<BonusCard>>> GetBonusCards()
        {
            if (_context.BonusCards == null)
            {
                return NotFound();
            }
            return await _context.BonusCards.Where(b => b.DeletedAt == null).ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{UserRoles.Cashier},{UserRoles.Admin}")]
        public async Task<ActionResult<BonusCard>> GetBonusCard(string id)
        {
            if (_context.BonusCards == null)
            {
                return NotFound();
            }
            var bonusCard = await _context.BonusCards.FindAsync(new Guid(id));

            if (bonusCard == null || bonusCard.DeletedAt != null)
            {
                return NotFound();
            }

            return bonusCard;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{UserRoles.Cashier},{UserRoles.Admin}")]
        public async Task<IActionResult> PutBonusCard(string id, BonusCard bonusCard)
        {
            Guid guid = new Guid(id);
            if (guid != bonusCard.Id)
            {
                return BadRequest();
            }

            _context.Entry(bonusCard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BonusCardExists(guid))
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
        public async Task<ActionResult<BonusCard>> PostBonusCard(BonusCard bonusCard)
        {
            if (_context.BonusCards == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BonusCards'  is null.");
            }
            _context.BonusCards.Add(bonusCard);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBonusCard", new { id = bonusCard.Id }, bonusCard);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteBonusCard(string id)
        {
            if (_context.BonusCards == null)
            {
                return NotFound();
            }
            var bonusCard = await _context.BonusCards.FindAsync(new Guid(id));
            if (bonusCard == null)
            {
                return NotFound();
            }

            _context.BonusCards.Remove(bonusCard);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BonusCardExists(Guid id)
        {
            return (_context.BonusCards?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
