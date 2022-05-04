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

        public struct BuySellData
        {
            public int id_stock { get; set; }
            public int stock_count { get; set; }
        }

        [Route("register")]
        public async Task<ActionResult<Users>> Register([FromBody]UsersDTO userDTO)
        {
            Users users = new Users();
            users = (Users)userDTO;
            _context.Users.Add(users);
            await _context.SaveChangesAsync();

            return users;
        }

        [Authorize]
        [Route("BuyStock")]
        public async Task<ActionResult<Bags>> BuyStock([FromBody] BuySellData _data)
        {
            Users _curUser = Services.Get_User(User.Identity.Name,_context);
            Stocks _curStock = Services.Get_Stock(_data.id_stock, _context);
            
            return Ok(await Services.Serv_BuyStock(_curUser, _curStock, _context, _data.id_stock, _data.stock_count));
        }

        [Authorize]
        [Route("SellStock")]
        public async Task<ActionResult<Bags>> SellStock([FromBody] BuySellData _data)
        {
            Users _curUser = Services.Get_User(User.Identity.Name, _context);
            Stocks _curStock = Services.Get_Stock(_data.id_stock, _context);

            return Ok(await Services.Serv_SellStock(_curUser, _curStock, _context, _data.id_stock, _data.stock_count));
        }


    }
}
