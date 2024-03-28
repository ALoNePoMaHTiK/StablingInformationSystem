using Microsoft.EntityFrameworkCore;
using StablingApi.Models;

namespace StablingApi.Contexts
{
    public class TrainingsContext : DbContext
    {
        public TrainingsContext(DbContextOptions<TrainingsContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Training>(builder =>
            {
                builder.ToTable(tb => tb.HasTrigger("TR_Trainings_InsteadInsert"));
                builder.ToTable(tb => tb.HasTrigger("TR_Trainings_AfterUpdate"));
            });
            modelBuilder.Entity<TrainingWithdrawing>().HasKey(w => new { w.TrainingId, w.BalanceWithdrawingId });
        }

        public DbSet<TrainingType> TrainingTypes { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<TrainingWithdrawing> TrainingWithdrawings { get; set; }
    }
}