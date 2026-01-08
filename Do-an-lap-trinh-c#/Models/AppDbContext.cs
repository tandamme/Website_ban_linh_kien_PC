using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Do_an_lap_trinh_c_.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
