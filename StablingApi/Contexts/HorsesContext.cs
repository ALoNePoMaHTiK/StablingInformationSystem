using Microsoft.EntityFrameworkCore;
using StablingApi.Models;

namespace StablingApi.Contexts
{
    public class HorsesContext : DbContext
    {
        public DbSet<Horse> Horses { get; set; }
        public HorsesContext(DbContextOptions<HorsesContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
