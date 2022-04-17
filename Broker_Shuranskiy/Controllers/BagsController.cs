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

        [Authorize]
        [HttpPost("BuyStock")]
        public async Task<ActionResult<Bags>> BuyStock(int id_stock, int stock_count)
        {
            Users _curUser = Get_User(User.Identity.Name);
            Stocks _curStock=Get_Stock(id_stock);
            Bags bags = new Bags();
            bags.Id_User = _curUser.Id;
            bags.Stock_Count = stock_count;
            bags.Id_Stock = id_stock;

            if(stock_count<_curStock.Min_Lot)
                return Ok($"Минимальный лот для покупки {_curStock.Min_Lot}");

            if (_curUser.Balance < _curStock.Price * stock_count)
                return Ok("Недостаточно средств");

            _curUser.Balance -= _curStock.Price * stock_count;
            int _count = CountBagsId(_curUser.Id, id_stock);
            if (_count == 0)
            {
                _context.Bags.Add(bags);
                await _context.SaveChangesAsync();
                _context.Entry(_curUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetBags", new { id = bags.Id }, bags);
            }
            else
            {
                Bags _curBag = Get_Bags(_curUser.Id, id_stock);
                _curBag.Stock_Count += stock_count;

                _context.Entry(_curBag).State = EntityState.Modified;
                _context.Entry(_curUser).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BagsExists(_curBag.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                bags = await _context.Bags.FindAsync(_curBag.Id);
                return bags;
            }
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
        private Bags Get_Bags(long user_id, long stock_id)
        {
            IQueryable<Bags> query = (from Bags in _context.Bags where Bags.Id_User == user_id && Bags.Id_Stock == stock_id select Bags);
            Bags bag = query.FirstOrDefault();
            return bag;
        }
        private int CountBagsId(long user_id,long stock_id)
        {
            int _count = (from Bags in _context.Bags where Bags.Id_User == user_id && Bags.Id_Stock==stock_id select Bags.Id).Count();
            return _count;
        }
        private Users Get_User(string name)
        {
            IQueryable<Users> query = (from Users in _context.Users where Users.User_Name == name select Users);
            Users _user = query.FirstOrDefault();
            return _user;
        }
        private Stocks Get_Stock(long stock_id)
        {
            IQueryable<Stocks> query = (from Stocks in _context.Stocks where Stocks.Id == stock_id select Stocks);
            Stocks _stock = query.FirstOrDefault();
            return _stock;
        }

    }
}
