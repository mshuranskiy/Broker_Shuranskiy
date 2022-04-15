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
    public class StocksController : ControllerBase
    {
        private readonly DataContext _context;

        public StocksController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Stocks
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stocks>>> GetStocks()
        {
            return await _context.Stocks.ToListAsync();
        }

        // GET: api/Stocks/5
        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Stocks>> GetStocks(long id)
        {
            var stocks = await _context.Stocks.FindAsync(id);

            if (stocks == null)
            {
                return NotFound();
            }

            return stocks;
        }

        // PUT: api/Stocks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStocks(long id, Stocks stocks)
        {
            if (id != stocks.Id)
            {
                return BadRequest();
            }

            _context.Entry(stocks).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StocksExists(id))
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

        // POST: api/Stocks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Stocks>> PostStocks(Stocks stocks)
        {
            _context.Stocks.Add(stocks);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStocks", new { id = stocks.Id }, stocks);
        }

        // DELETE: api/Stocks/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStocks(long id)
        {
            var stocks = await _context.Stocks.FindAsync(id);
            if (stocks == null)
            {
                return NotFound();
            }

            _context.Stocks.Remove(stocks);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StocksExists(long id)
        {
            return _context.Stocks.Any(e => e.Id == id);
        }
    }
}
