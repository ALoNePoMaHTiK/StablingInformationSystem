using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StablingApi.Models;

namespace StablingApi.Contexts
{
    public class MoneyContext : DbContext
    {
        public DbSet<MoneyAccount> MoneyAccounts { get; set; }
        public DbSet<MoneyTransaction> MoneyTransactions { get; set; }
        public DbSet<MoneyTransactionForShow> MoneyTransactionsForShow { get; set; }
        public DbSet<BusinessOperation> BusinessOperations { get; set; }
        public DbSet<BusinessOperationType> BusinessOperationTypes { get; set; }
        public DbSet<BusinessOperationForShow> BusinessOperationsForShow { get; set; }
        public DbSet<BalanceReplenishment> BalanceReplenishments { get; set; }
        public DbSet<BalanceReplenishmentForShow> BalanceReplenishmentsForShow { get; set; }

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
            modelBuilder.Entity<MoneyTransactionForShow>(table =>
            {
                table.HasNoKey();
                table.ToView("VW_MoneyTransactionsForShow");
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
            modelBuilder.Entity<BalanceReplenishmentForShow>(table =>
            {
                table.HasNoKey();
                table.ToView("VW_BalanceReplenishmentsForShow");
            });

            modelBuilder.Entity<BalanceReplenishment>(builder =>
            {
                builder.ToTable(tb => tb.HasTrigger("TR_BalanceReplenishments_AfterInsert"));
            });
            
        }
    }
}
