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
        public DbSet<BusinessOperationForShow> BusinessOperationsForShow { get; set; }
        public DbSet<BalanceReplenishment> BalanceReplenishments { get; set; }

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

            modelBuilder.Entity<BusinessOperation>(builder =>
            {
                builder.ToTable(tb => tb.HasTrigger("TR_BalanceWithdrawings_AfterInsert"));
            });
            modelBuilder.Entity<BusinessOperationForShow>((table =>
            {
                table.HasNoKey();
                table.ToView("VW_BusinessOperationsForShow");
            }));
        }
    }
}
