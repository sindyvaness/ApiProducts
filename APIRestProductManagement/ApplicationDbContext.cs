using APIRestProductManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIRestProductManagement
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
    }
}
