using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class SqlContext : DbContext
    {
        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
        }

        public DbSet<Amostra> Amostras { get; set; }
    }
}
