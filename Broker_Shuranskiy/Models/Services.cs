using Broker_Shuranskiy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Broker_Shuranskiy.Models
{
    public class Services
    {
        public static async Task<object> Serv_BuyStock(Users _curUser, Stocks _curStock, DataContext _context, int id_stock, int stock_count)
        {
            Bags bags = new Bags
            {
                Id_User = _curUser.Id,
                Stock_Count = stock_count,
                Id_Stock = id_stock
            };
            if (stock_count < _curStock.Min_Lot)
                return ($"Минимальный лот для покупки {_curStock.Min_Lot}");

            if (_curUser.Balance < _curStock.Price * stock_count)
                return ("Недостаточно средств");

            _curUser.Balance -= _curStock.Price * stock_count;
            int _count = CountBagsId(_curUser.Id, id_stock,_context);
            if (_count == 0)
            {
                _context.Bags.Add(bags);
                await _context.SaveChangesAsync();
                _context.Entry(_curUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return bags;
            }
            else
            {
                Bags _curBag = Get_Bags(_curUser.Id, id_stock,_context);
                _curBag.Stock_Count += stock_count;

                _context.Entry(_curBag).State = EntityState.Modified;
                _context.Entry(_curUser).State = EntityState.Modified;

                await _context.SaveChangesAsync();
               
                bags = await _context.Bags.FindAsync(_curBag.Id);
                return bags;
            }
        }

        public static async Task<object> Serv_SellStock(Users _curUser, Stocks _curStock, DataContext _context, int id_stock, int stock_count)
        {
            Bags bags = new Bags
            {
                Id_User = _curUser.Id,
                Stock_Count = stock_count,
                Id_Stock = id_stock
            };

            if (stock_count < _curStock.Min_Lot)
                return ($"Минимальный лот для покупки {_curStock.Min_Lot}");

            _curUser.Balance += _curStock.Price * stock_count;
            int _count = Services.CountBagsId(_curUser.Id, id_stock, _context);
            if (_count == 0)
            {
                return ("У вас нет данной акции");
            }
            else
            {
                Bags _curBag = Services.Get_Bags(_curUser.Id, id_stock, _context);
                if (_curBag.Stock_Count < stock_count)
                    return ("Недостаточно акций для продажи");
                _curBag.Stock_Count -= stock_count;
                if (_curBag.Stock_Count == 0)
                {
                    _context.Bags.Remove(_curBag);
                    await _context.SaveChangesAsync();
                    return ("Все акции успешно проданы");
                }

                _context.Entry(_curBag).State = EntityState.Modified;
                _context.Entry(_curUser).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                
                bags = await _context.Bags.FindAsync(_curBag.Id);
                return bags;
            }
        }

        public static bool BagsExists(long id, DataContext _context)
        {
            return _context.Bags.Any(e => e.Id == id);
        }

        public static Bags Get_Bags(long user_id, long stock_id, DataContext _context)
        {
            IQueryable<Bags> query = (from Bags in _context.Bags where Bags.Id_User == user_id && Bags.Id_Stock == stock_id select Bags);
            Bags bag = query.FirstOrDefault();
            return bag;
        }
        public static int CountBagsId(long user_id, long stock_id, DataContext _context)
        {
            int _count = (from Bags in _context.Bags where Bags.Id_User == user_id && Bags.Id_Stock == stock_id select Bags.Id).Count();
            return _count;
        }
        public static Users Get_User(string name, DataContext _context)
        {
            IQueryable<Users> query = (from Users in _context.Users where Users.User_Name == name select Users);
            Users _user = query.FirstOrDefault();
            return _user;
        }
        public static Stocks Get_Stock(long stock_id, DataContext _context)
        {
            IQueryable<Stocks> query = (from Stocks in _context.Stocks where Stocks.Id == stock_id select Stocks);
            Stocks _stock = query.FirstOrDefault();
            return _stock;
        }
    }
}
