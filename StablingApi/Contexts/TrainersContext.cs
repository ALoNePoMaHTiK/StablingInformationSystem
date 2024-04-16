using Microsoft.EntityFrameworkCore;
using StablingApi.Models;

namespace StablingApi.Contexts
{
    public class TrainersContext : DbContext
    {
        public TrainersContext(DbContextOptions<TrainersContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Trainer> Trainers { get; set; }
    }
}
