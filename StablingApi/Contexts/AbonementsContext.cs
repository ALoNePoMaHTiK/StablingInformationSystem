using Microsoft.EntityFrameworkCore;
using StablingApi.Models;

namespace StablingApi.Contexts
{
    public class AbonementsContext : DbContext
    {
        public AbonementsContext(DbContextOptions<AbonementsContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Abonement>(builder =>
            {
                builder.ToTable(tb => tb.HasTrigger("TR_Abonements_InsteadInsert"));
            });
            modelBuilder.Entity<AbonementWithdrawing>().HasKey(w => new { w.AbonementId, w.BalanceWithdrawingId });

            modelBuilder.Entity<AbonementForShow>(builder =>
            {
                builder.ToView("VW_AbonementsForShow");
                builder.HasNoKey();
            });
        }

        public DbSet<Abonement> Abonements { get; set; }
        public DbSet<AbonementForShow> AbonementsForShow { get; set; }
        public DbSet<AbonementType> AbonementTypes { get; set; }
        public DbSet<AbonementMark> AbonementMarks { get; set; }
        public DbSet<AbonementWithdrawing> AbonementWithdrawings { get; set; }
    }
}