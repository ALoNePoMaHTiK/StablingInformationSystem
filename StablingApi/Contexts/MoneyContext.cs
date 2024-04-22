using Microsoft.EntityFrameworkCore;
using StablingApi.Models;

namespace StablingApi.Contexts
{
    public class MoneyContext : DbContext
    {
        public DbSet<MoneyAccount> MoneyAccounts { get; set; }
        public DbSet<MoneyTransaction> MoneyTransactions { get; set; }
        public DbSet<BusinessOperation> BusinessOperations { get; set; }
        public DbSet<BusinessOperationType> BusinessOperationTypes { get; set; }
        public DbSet<BusinessOperationView> BusinessOperationViews { get; set; }
        public MoneyContext(DbContextOptions<MoneyContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MoneyTransaction>(builder =>
            {
                builder.ToTable(tb => tb.HasTrigger("TR_MoneyTransactions_AfterInsert"));
            });

            modelBuilder.Entity<BusinessOperationGroupView>((pc =>
            {
                pc.ToView("VW_BusinessOperationsByIncome");
            }));

            modelBuilder.Entity<BusinessOperation>(builder =>
            {
                builder.ToTable(tb => tb.HasTrigger("TR_BalanceWithdrawings_AfterInsert"));
            });
        }
    }
}
