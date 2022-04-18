using Broker_Shuranskiy.Data;
using Broker_Shuranskiy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Broker_Shuranskiy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly DataContext _context;

        public ServicesController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Users>> Register(string First_Name, string Second_Name, string User_Name, string Password)
        {
            Users users = new Users();
            users.First_Name = First_Name;
            users.Second_Name = Second_Name;
            users.User_Name = User_Name;
            users.Password = Password;
            _context.Users.Add(users);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = users.Id }, users);
        }

        [Authorize]
        [HttpPost("BuyStock")]
        public async Task<ActionResult<Bags>> BuyStock(int id_stock, int stock_count)
        {
            Users _curUser = Get_User(User.Identity.Name);
            Stocks _curStock = Get_Stock(id_stock);
            Bags bags = new Bags();
            bags.Id_User = _curUser.Id;
            bags.Stock_Count = stock_count;
            bags.Id_Stock = id_stock;

            if (stock_count < _curStock.Min_Lot)
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
        private int CountBagsId(long user_id, long stock_id)
        {
            int _count = (from Bags in _context.Bags where Bags.Id_User == user_id && Bags.Id_Stock == stock_id select Bags.Id).Count();
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

        private long GetUserID(string name)
        {
            IQueryable<long> query = (from Users in _context.Users where Users.User_Name == name select Users.Id);
            long id = query.FirstOrDefault();
            return id;
        }


    }
}
