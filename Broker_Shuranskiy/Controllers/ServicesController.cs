using Broker_Shuranskiy.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
      
    }
}
