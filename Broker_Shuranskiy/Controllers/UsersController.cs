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
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(long id)
        {
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(long id, Users users)
        {
            long _UserID = GetUserID(User.Identity.Name);

            if (id != users.Id || _UserID != id) 
            {
                return BadRequest();
            }

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers(Users users)
        {
            _context.Users.Add(users);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = users.Id }, users);
        }

        
        [HttpPost("register")]
        public async Task<ActionResult<Users>> Register(string First_Name, string Second_Name,string User_Name, string Password)
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

        // DELETE: api/Users/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(long id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private long GetUserID(string name)
        {
            IQueryable<long> query = (from Users in _context.Users where Users.User_Name == name select Users.Id);
            long id = query.FirstOrDefault();
            return id;
        }

    }

    
}
