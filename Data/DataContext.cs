using jumpstart_ud.Models;
using Microsoft.EntityFrameworkCore;

namespace jumpstart_ud.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Character> Characters { get; set; }
    }
}
 