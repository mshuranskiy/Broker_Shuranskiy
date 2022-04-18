using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Broker_Shuranskiy.Data;
using Broker_Shuranskiy.Models;
using Microsoft.AspNetCore.Authorization;

namespace Broker_Shuranskiy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BagsController : ControllerBase
    {
        private readonly DataContext _context;

        public BagsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Bags
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bags>>> GetBags()
        {
            return await _context.Bags.ToListAsync();
        }

        // GET: api/Bags/5
        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Bags>> GetBags(long id)
        {
            var bags = await _context.Bags.FindAsync(id);

            if (bags == null)
            {
                return NotFound();
            }

            return bags;
        }

        // PUT: api/Bags/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBags(long id, Bags bags)
        {
            if (id != bags.Id)
            {
                return BadRequest();
            }

            _context.Entry(bags).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BagsExists(id))
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

        // POST: api/Bags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Bags>> PostBags(Bags bags)
        {
            _context.Bags.Add(bags);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBags", new { id = bags.Id }, bags);
        }


        // DELETE: api/Bags/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBags(long id)
        {
            var bags = await _context.Bags.FindAsync(id);
            if (bags == null)
            {
                return NotFound();
            }

            _context.Bags.Remove(bags);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BagsExists(long id)
        {
            return _context.Bags.Any(e => e.Id == id);
        }
        

    }
}
