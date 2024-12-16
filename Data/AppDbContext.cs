using Allurerealstate.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Allurerealstate.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Property> Properties { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
