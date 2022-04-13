using Microsoft.EntityFrameworkCore;
using Broker_Shuranskiy.Models;

namespace Broker_Shuranskiy.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Stocks> Stocks { get; set; }
        public DbSet<Bags> Bags { get; set; }
    }
}
