using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace RMSClient.Models.Inventory
{
    public partial class InventoryContext : DbContext
    {
        public InventoryContext()
        {
        }

        public InventoryContext(DbContextOptions<InventoryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Table1> Table1s { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
#if DEBUG
        optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=Inventory;Trusted_Connection=True;"); // var constr = Environment.UserName.Contains("lex.pi") ? "Server=.\\sqlexpress;Database=Inventory;Trusted_Connection=True;" : "Server=MTdevSQLDB;Database=Inventory;Trusted_Connection=True;";        optionsBuilder.UseSqlServer(constr);        System.Diagnostics.Debug.WriteLine($" ■ ■ ■ {constr}");
#else
        optionsBuilder.UseSqlServer("Server=MTdevSQLDB;Database=Inventory;Trusted_Connection=True;");
#endif
      }
    }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.HasIndex(e => e.ShortName, "Constraint_ShortName")
                    .IsUnique();

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountTypes)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AcctBrokerId)
                    .HasColumnName("acctBrokerID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AcctTypeId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("AcctTypeID");

                entity.Property(e => e.AdpAccountCd)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("adp_account_cd")
                    .IsFixedLength(true);

                entity.Property(e => e.AdpBranchCd)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("adp_branch_cd")
                    .IsFixedLength(true);

                entity.Property(e => e.AdpChk)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("adp_chk")
                    .IsFixedLength(true);

                entity.Property(e => e.AdpTypeAccountCd)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("adp_type_account_cd")
                    .IsFixedLength(true);

                entity.Property(e => e.BmoaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BMOAccountID");

                entity.Property(e => e.BorRateId)
                    .HasColumnName("BorRateID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CanaccordAccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CanaccordAccountID");

                entity.Property(e => e.DefaultAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultBook)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FxRateId)
                    .HasColumnName("FxRateID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IntRateId)
                    .HasColumnName("IntRateID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OverrideAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.PensonAccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonAccountID");

                entity.Property(e => e.PensonGiveUpId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonGiveUpID");

                entity.Property(e => e.PensonUsaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonUSAccountID");

                entity.Property(e => e.SecGroupId)
                    .HasColumnName("SecGroupID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ShortName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TsxStampId)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");

                entity.Property(e => e.BookId).HasColumnName("Book_ID");

                entity.Property(e => e.AccountNum)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DefMarket)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReportingCurrency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Table1>(entity =>
            {
                entity.ToTable("Table_1");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Desc)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
