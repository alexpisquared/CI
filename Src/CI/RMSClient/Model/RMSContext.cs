using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace RMSClient.Model
{
  public partial class RMSContext : DbContext
  {
    public RMSContext()
    {
    }

    public RMSContext(DbContextOptions<RMSContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Action> Actions { get; set; }
    public virtual DbSet<DisableList> DisableLists { get; set; }
    public virtual DbSet<Request> Requests { get; set; }
    public virtual DbSet<RequestHistory> RequestHistories { get; set; }
    public virtual DbSet<RequestType> RequestTypes { get; set; }
    public virtual DbSet<Response> Responses { get; set; }
    public virtual DbSet<Source> Sources { get; set; }
    public virtual DbSet<Status> Statuses { get; set; }
    public virtual DbSet<SubType> SubTypes { get; set; }
    public virtual DbSet<UpdateType> UpdateTypes { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        var constr = Environment.UserName.Contains("lex.pi") ? "Server=.\\sqlexpress;Database=BR;Trusted_Connection=True;" : "Server=mtdevsqldb;Database=RMS;Trusted_Connection=True;";
        optionsBuilder.UseSqlServer(constr);        System.Diagnostics.Debug.WriteLine($" ■ ■ ■ {constr}");
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

      modelBuilder.Entity<Action>(entity =>
      {
        entity.HasNoKey();

        entity.ToTable("Action");

        entity.Property(e => e.ActionId).HasColumnName("ActionID");

        entity.Property(e => e.Name)
                  .HasMaxLength(15)
                  .IsUnicode(false);

        entity.Property(e => e.SubTypeId).HasColumnName("SubTypeID");

        entity.Property(e => e.TypeId).HasColumnName("TypeID");
      });

      modelBuilder.Entity<DisableList>(entity =>
      {
        entity.HasKey(e => e.Symbol);

        entity.ToTable("DisableList");

        entity.Property(e => e.Symbol)
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .HasColumnName("symbol");
      });

      modelBuilder.Entity<Request>(entity =>
      {
              //..jshdgfdjsg 1/2 entity.HasNoKey(); 

              entity.ToTable("Request");

        entity.Property(e => e.AccountId).HasColumnName("AccountID");

        entity.Property(e => e.ActionId).HasColumnName("ActionID");

        entity.Property(e => e.Bbsnote)
                  .HasMaxLength(100)
                  .IsUnicode(false)
                  .HasColumnName("BBSNote");

        entity.Property(e => e.ClientRequestId).HasColumnName("ClientRequestID");

        entity.Property(e => e.CompoundFreq)
                  .HasMaxLength(1)
                  .IsUnicode(false)
                  .IsFixedLength(true);

        entity.Property(e => e.CreationDate).HasColumnType("datetime");

        entity.Property(e => e.CreatorId).HasColumnName("CreatorID");

        entity.Property(e => e.CurrencyCode)
                  .HasMaxLength(3)
                  .IsUnicode(false)
                  .IsFixedLength(true);

        entity.Property(e => e.Cusip)
                  .HasMaxLength(10)
                  .IsUnicode(false)
                  .HasColumnName("CUSIP");

        entity.Property(e => e.OtherInfo)
                  .HasMaxLength(100)
                  .IsUnicode(false);

        entity.Property(e => e.PaymentFreq)
                  .HasMaxLength(1)
                  .IsUnicode(false)
                  .IsFixedLength(true);

        entity.Property(e => e.QuantityType)
                  .HasMaxLength(1)
                  .IsUnicode(false)
                  .IsFixedLength(true);

        entity.Property(e => e.Rate)
                  .HasMaxLength(20)
                  .IsUnicode(false);

        entity.Property(e => e.RequestId).HasColumnName("RequestID");

        entity.Property(e => e.SecAdpnumber)
                  .HasMaxLength(7)
                  .IsUnicode(false)
                  .HasColumnName("SecADPNumber")
                  .IsFixedLength(true);

        entity.Property(e => e.SourceId).HasColumnName("SourceID");

        entity.Property(e => e.StatusId).HasColumnName("StatusID");

        entity.Property(e => e.SubTypeId).HasColumnName("SubTypeID");

        entity.Property(e => e.Symbol)
                  .HasMaxLength(50)
                  .IsUnicode(false);

        entity.Property(e => e.Term)
                  .HasMaxLength(20)
                  .IsUnicode(false);

        entity.Property(e => e.TypeId).HasColumnName("TypeID");

        entity.Property(e => e.UpdateDate).HasColumnType("datetime");

        entity.Property(e => e.UserId).HasColumnName("UserID");
      });

      modelBuilder.Entity<RequestHistory>(entity =>
      {
        entity.HasNoKey();

        entity.ToTable("RequestHistory");

        entity.Property(e => e.Bbsnote)
                  .HasMaxLength(100)
                  .IsUnicode(false)
                  .HasColumnName("BBSNote");

        entity.Property(e => e.ParentId).HasColumnName("ParentID");

        entity.Property(e => e.RequestHistoryId).HasColumnName("RequestHistoryID");

        entity.Property(e => e.RequestId).HasColumnName("RequestID");

        entity.Property(e => e.StatusId).HasColumnName("StatusID");

        entity.Property(e => e.TypeId).HasColumnName("TypeID");

        entity.Property(e => e.UpdateTime).HasColumnType("datetime");

        entity.Property(e => e.UserId).HasColumnName("UserID");
      });

      modelBuilder.Entity<RequestType>(entity =>
      {
        entity.HasNoKey();

        entity.ToTable("RequestType");

        entity.Property(e => e.Name)
                  .HasMaxLength(20)
                  .IsUnicode(false);

        entity.Property(e => e.TypeId).HasColumnName("TypeID");
      });

      modelBuilder.Entity<Response>(entity =>
      {
        entity.HasKey(e => e.RespId)
                  .HasName("PK_RESPONSE");

        entity.ToTable("Response");

        entity.Property(e => e.RespId)
                  .ValueGeneratedNever()
                  .HasColumnName("RespID");

        entity.Property(e => e.Cause)
                  .HasMaxLength(256)
                  .IsUnicode(false);

        entity.Property(e => e.RespText)
                  .HasMaxLength(128)
                  .IsUnicode(false);
      });

      modelBuilder.Entity<Source>(entity =>
      {
        entity.HasNoKey();

        entity.ToTable("Source");

        entity.Property(e => e.SourceId).HasColumnName("SourceID");

        entity.Property(e => e.SourceName)
                  .HasMaxLength(20)
                  .IsUnicode(false);
      });

      modelBuilder.Entity<Status>(entity =>
      {
        entity.HasNoKey();

        entity.ToTable("Status");

        entity.Property(e => e.Name)
                  .HasMaxLength(20)
                  .IsUnicode(false);

        entity.Property(e => e.StatusId).HasColumnName("StatusID");
      });

      modelBuilder.Entity<SubType>(entity =>
      {
        entity.HasNoKey();

        entity.ToTable("SubType");

        entity.Property(e => e.Name)
                  .HasMaxLength(20)
                  .IsUnicode(false);

        entity.Property(e => e.SubTypeId).HasColumnName("SubTypeID");

        entity.Property(e => e.TypeId).HasColumnName("TypeID");
      });

      modelBuilder.Entity<UpdateType>(entity =>
      {
        entity.HasNoKey();

        entity.ToTable("UpdateType");

        entity.Property(e => e.Name)
                  .HasMaxLength(30)
                  .IsUnicode(false);

        entity.Property(e => e.TypeId).HasColumnName("TypeID");
      });

      modelBuilder.Entity<User>(entity =>
      {
        entity.HasNoKey();

        entity.ToTable("User");

        entity.Property(e => e.Password)
                  .HasMaxLength(32)
                  .IsUnicode(false);

        entity.Property(e => e.UserId).HasColumnName("UserID");

        entity.Property(e => e.UserName)
                  .HasMaxLength(128)
                  .IsUnicode(false);
      });

      OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
  }
}
