﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace zEfPoc.Model
{
    public partial class OneBaseContext : DbContext
    {
        public OneBaseContext()
        {
        }

        public OneBaseContext(DbContextOptions<OneBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppStng> AppStngs { get; set; }
        public virtual DbSet<Audition> Auditions { get; set; }
        public virtual DbSet<LkuLanguage> LkuLanguages { get; set; }
        public virtual DbSet<LkuLevel> LkuLevels { get; set; }
        public virtual DbSet<LkuSubject> LkuSubjects { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Problem> Problems { get; set; }
        public virtual DbSet<SessionResult> SessionResults { get; set; }
        public virtual DbSet<Sysdiagram> Sysdiagrams { get; set; }
        public virtual DbSet<TombStone> TombStones { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<VwEventUserUtc> VwEventUserUtcs { get; set; }
        public virtual DbSet<VwLast100> VwLast100s { get; set; }
        public virtual DbSet<VwUser> VwUsers { get; set; }
        public virtual DbSet<VwUserHopsUtc> VwUserHopsUtcs { get; set; }
        public virtual DbSet<WebEventLog> WebEventLogs { get; set; }
        public virtual DbSet<WebsiteUser> WebsiteUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=OneBase;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AppStng>(entity =>
            {
                entity.ToTable("AppStng", "TCh");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.Property(e => e.Note).HasColumnType("text");

                entity.Property(e => e.ProLtgl).HasColumnName("ProLTgl");

                entity.Property(e => e.SubLesnId).HasMaxLength(8);

                entity.Property(e => e.UserId).HasMaxLength(16);
            });

            modelBuilder.Entity<Audition>(entity =>
            {
                entity.ToTable("Audition", "SpB");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DoneAt).HasColumnType("datetime");

                entity.Property(e => e.PlayerId)
                    .HasMaxLength(128)
                    .HasColumnName("Player_ID");

                entity.Property(e => e.ProblemId).HasColumnName("Problem_ID");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Auditions)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("FK_dbo.Audition_dbo.Player_Player_ID");

                entity.HasOne(d => d.Problem)
                    .WithMany(p => p.Auditions)
                    .HasForeignKey(d => d.ProblemId)
                    .HasConstraintName("FK_dbo.Audition_dbo.Problem_Problem_ID");
            });

            modelBuilder.Entity<LkuLanguage>(entity =>
            {
                entity.ToTable("LkuLanguage", "SpB");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<LkuLevel>(entity =>
            {
                entity.ToTable("LkuLevel", "SpB");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<LkuSubject>(entity =>
            {
                entity.ToTable("LkuSubject", "SpB");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Player", "SpB");

                entity.Property(e => e.Id)
                    .HasMaxLength(128)
                    .HasColumnName("ID");

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Problem>(entity =>
            {
                entity.ToTable("Problem", "SpB");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.AddedBy).HasMaxLength(24);

                entity.Property(e => e.BatchSource).HasMaxLength(24);

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(24);

                entity.Property(e => e.HintMessage).HasMaxLength(64);

                entity.Property(e => e.LanguageId).HasColumnName("Language_ID");

                entity.Property(e => e.LevelId).HasColumnName("Level_ID");

                entity.Property(e => e.Notes).HasMaxLength(128);

                entity.Property(e => e.ProblemText).HasMaxLength(24);

                entity.Property(e => e.SolutionText).HasMaxLength(24);

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.Problems)
                    .HasForeignKey(d => d.LanguageId)
                    .HasConstraintName("FK_dbo.Problem_dbo.LkuLanguage_Language_ID");

                entity.HasOne(d => d.Level)
                    .WithMany(p => p.Problems)
                    .HasForeignKey(d => d.LevelId)
                    .HasConstraintName("FK_dbo.Problem_dbo.LkuLevel_Level_ID");
            });

            modelBuilder.Entity<SessionResult>(entity =>
            {
                entity.ToTable("SessionResult", "TCh");

                entity.Property(e => e.DoneAt).HasColumnType("datetime");

                entity.Property(e => e.ExcerciseName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Note).HasColumnType("text");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SessionResults)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.SessionResult_dbo.User_UserId");
            });

            modelBuilder.Entity<Sysdiagram>(entity =>
            {
                entity.HasKey(e => e.DiagramId)
                    .HasName("PK__sysdiagr__C2B05B6147B91772");

                entity.ToTable("sysdiagrams");

                entity.HasIndex(e => new { e.PrincipalId, e.Name }, "UK_principal_name")
                    .IsUnique();

                entity.Property(e => e.DiagramId).HasColumnName("diagram_id");

                entity.Property(e => e.Definition).HasColumnName("definition");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.Property(e => e.PrincipalId).HasColumnName("principal_id");

                entity.Property(e => e.Version).HasColumnName("version");
            });

            modelBuilder.Entity<TombStone>(entity =>
            {
                entity.ToTable("TombStone", "SpB");

                entity.Property(e => e.Id)
                    .HasMaxLength(128)
                    .HasColumnName("ID");

                entity.Property(e => e.PlayerId)
                    .HasMaxLength(128)
                    .HasColumnName("Player_ID");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.TombStones)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("FK_dbo.TombStone_dbo.Player_Player_ID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "TCh");

                entity.Property(e => e.UserId).HasMaxLength(16);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.Property(e => e.Note).HasColumnType("text");
            });

            modelBuilder.Entity<VwEventUserUtc>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_EventUserUtc", "APi");

                entity.Property(e => e.DoneAt).HasColumnType("datetime");

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.Nickname).HasMaxLength(32);
            });

            modelBuilder.Entity<VwLast100>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Last100", "APi");

                entity.Property(e => e.DoneAtLocalTime)
                    .HasColumnType("datetime")
                    .HasColumnName("DoneAt LocalTime");

                entity.Property(e => e.EventData)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Nickname).HasMaxLength(32);
            });

            modelBuilder.Entity<VwUser>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_User", "APi");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.EventData)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.LastEvEst).HasColumnType("datetime");

                entity.Property(e => e.Nickname).HasMaxLength(32);
            });

            modelBuilder.Entity<VwUserHopsUtc>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_UserHopsUtc", "APi");

                entity.Property(e => e.Finished).HasColumnType("datetime");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Nickname).HasMaxLength(32);

                entity.Property(e => e.ReviewedAt).HasColumnType("datetime");

                entity.Property(e => e.Started).HasColumnType("datetime");
            });

            modelBuilder.Entity<WebEventLog>(entity =>
            {
                entity.ToTable("WebEventLog", "APi");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DoneAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.WebsiteUserId).HasColumnName("WebsiteUserID");

                entity.HasOne(d => d.WebsiteUser)
                    .WithMany(p => p.WebEventLogs)
                    .HasForeignKey(d => d.WebsiteUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WebEventLog_WebsiteUser");
            });

            modelBuilder.Entity<WebsiteUser>(entity =>
            {
                entity.ToTable("WebsiteUser", "APi");

                entity.HasIndex(e => e.EventData, "IX_WebsiteUser")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DoNotLog).HasDefaultValueSql("((0))");

                entity.Property(e => e.EventData).IsRequired();

                entity.Property(e => e.LastVisitAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nickname).HasMaxLength(32);

                entity.Property(e => e.Note).HasColumnType("ntext");

                entity.Property(e => e.ReviewedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
