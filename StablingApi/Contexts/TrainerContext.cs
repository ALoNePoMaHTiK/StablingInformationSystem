using Microsoft.EntityFrameworkCore;
using StablingApi.Models;

namespace StablingApi.Contexts
{
    public class TrainerContext : DbContext
    {
        public TrainerContext(DbContextOptions<TrainerContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Trainer> Trainers { get; set; }
    }
}
