using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace RMSClient.Models.BR
{
  public partial class BRContext : DbContext
  {
    readonly string _connectoinString;

    public BRContext(string connectoinString)
    {
      _connectoinString = connectoinString;
    }

    public BRContext(DbContextOptions<BRContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        optionsBuilder.UseSqlServer(_connectoinString);
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

      modelBuilder.Entity<Account>(entity =>
      {
        entity.ToTable("account");

        entity.Property(e => e.AccountId).HasColumnName("account_id");

        entity.Property(e => e.AccountCd)
                  .IsRequired()
                  .HasMaxLength(5)
                  .IsUnicode(false)
                  .HasColumnName("account_cd")
                  .IsFixedLength(true);

        entity.Property(e => e.AccountNum)
                  .IsRequired()
                  .HasMaxLength(9)
                  .IsUnicode(false)
                  .HasComputedColumnSql("(([branch_cd]+[account_cd])+[type_account_cd])", false)
                  .IsFixedLength(true);

        entity.Property(e => e.AccountReferralPersonExternal).IsUnicode(false);

        entity.Property(e => e.AccountReferralPersonInternal).IsUnicode(false);

        entity.Property(e => e.AdGroup).IsUnicode(false);

        entity.Property(e => e.AdvertisementSource).IsUnicode(false);

        entity.Property(e => e.AtonRefId)
                  .HasMaxLength(100)
                  .IsUnicode(false)
                  .HasColumnName("aton_ref_id");

        entity.Property(e => e.BranchCd)
                  .IsRequired()
                  .HasMaxLength(3)
                  .IsUnicode(false)
                  .HasColumnName("branch_cd")
                  .IsFixedLength(true);

        entity.Property(e => e.BuyingPowerMultiple)
                  .HasColumnType("decimal(17, 2)")
                  .HasColumnName("buying_power_multiple");

        entity.Property(e => e.CampaignName).IsUnicode(false);

        entity.Property(e => e.ClientId).HasColumnName("client_id");

        entity.Property(e => e.ClientStatus).IsUnicode(false);

        entity.Property(e => e.ClosedDt)
                  .HasColumnType("datetime")
                  .HasColumnName("closed_dt");

        entity.Property(e => e.CmmsnToBrokerCd)
                  .IsRequired()
                  .HasMaxLength(1)
                  .IsUnicode(false)
                  .HasColumnName("cmmsn_to_broker_cd")
                  .HasDefaultValueSql("('Y')");

        entity.Property(e => e.CodCuid)
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .HasColumnName("cod_cuid");

        entity.Property(e => e.CodOthBrkrAccountNum)
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .HasColumnName("cod_oth_brkr_account_num");

        entity.Property(e => e.EDocumentAccountOpening).HasColumnName("eDocumentAccountOpening");

        entity.Property(e => e.HouseMarginType)
                  .IsUnicode(false)
                  .HasColumnName("house_margin_type")
                  .HasDefaultValueSql("('DEFAULT')");

        entity.Property(e => e.Jurisdiction).HasMaxLength(100);

        entity.Property(e => e.LeverageDisallowedCd)
                  .IsRequired()
                  .HasMaxLength(1)
                  .IsUnicode(false)
                  .HasColumnName("leverage_disallowed_cd")
                  .HasDefaultValueSql("('0')")
                  .IsFixedLength(true);

        entity.Property(e => e.NicknameTxt)
                  .HasMaxLength(200)
                  .IsUnicode(false)
                  .HasColumnName("nickname_txt");

        entity.Property(e => e.OpenedDt)
                  .HasColumnType("datetime")
                  .HasColumnName("opened_dt");

        entity.Property(e => e.PlanTypeCd)
                  .HasMaxLength(1)
                  .IsUnicode(false)
                  .HasColumnName("plan_type_cd")
                  .IsFixedLength(true);

        entity.Property(e => e.PromotionCodeOnOpeningAccount).IsUnicode(false);

        entity.Property(e => e.ReferralSite).IsUnicode(false);

        entity.Property(e => e.RepCd)
                  .IsUnicode(false)
                  .HasColumnName("rep_cd");

        entity.Property(e => e.RespAcesgFlg)
                  .HasMaxLength(1)
                  .IsUnicode(false)
                  .HasColumnName("resp_acesg_flg")
                  .HasDefaultValueSql("('N')")
                  .IsFixedLength(true);

        entity.Property(e => e.RespCesgFlg)
                  .HasMaxLength(1)
                  .IsUnicode(false)
                  .HasColumnName("resp_cesg_flg")
                  .HasDefaultValueSql("('N')")
                  .IsFixedLength(true);

        entity.Property(e => e.RespClbFlg)
                  .HasMaxLength(1)
                  .IsUnicode(false)
                  .HasColumnName("resp_clb_flg")
                  .HasDefaultValueSql("('N')")
                  .IsFixedLength(true);

        entity.Property(e => e.SpsEverInd)
                  .HasMaxLength(1)
                  .IsUnicode(false)
                  .HasColumnName("sps_ever_ind")
                  .IsFixedLength(true);

        entity.Property(e => e.StatusCd)
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .HasColumnName("status_cd")
                  .HasDefaultValueSql("('OPEN')");

        entity.Property(e => e.StmtPrintFlg)
                  .IsRequired()
                  .HasMaxLength(1)
                  .IsUnicode(false)
                  .HasColumnName("stmt_print_flg")
                  .HasDefaultValueSql("('N')")
                  .IsFixedLength(true);

        entity.Property(e => e.SubTypeCd)
                  .IsRequired()
                  .HasMaxLength(30)
                  .IsUnicode(false)
                  .HasColumnName("sub_type_cd");

        entity.Property(e => e.TimeAccountOpeningStartedByClient).HasColumnType("datetime");

        entity.Property(e => e.TimeAccountWasApproved).HasColumnType("datetime");

        entity.Property(e => e.TypeAccountCd)
                  .IsRequired()
                  .HasMaxLength(1)
                  .IsUnicode(false)
                  .HasColumnName("type_account_cd")
                  .IsFixedLength(true);

        entity.Property(e => e.UpdtNm)
                  .HasMaxLength(100)
                  .IsUnicode(false)
                  .HasColumnName("updt_nm");

        entity.Property(e => e.UpdtTmstp)
                  .HasColumnType("datetime")
                  .HasColumnName("updt_tmstp");

        entity.Property(e => e.WhereClientHeardAboutUs).IsUnicode(false);
      });

      OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
  }
}
