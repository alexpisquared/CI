using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DB.Inventory.Dbo.Models
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
        public virtual DbSet<Account20160405> Account20160405s { get; set; }
        public virtual DbSet<AccountAdpView> AccountAdpViews { get; set; }
        public virtual DbSet<AccountAdpView2> AccountAdpView2s { get; set; }
        public virtual DbSet<AccountAdpViewEx> AccountAdpViewices { get; set; }
        public virtual DbSet<AccountBackup> AccountBackups { get; set; }
        public virtual DbSet<AccountCurrencyDetail> AccountCurrencyDetails { get; set; }
        public virtual DbSet<AccountGroup> AccountGroups { get; set; }
        public virtual DbSet<AccountGroupMember> AccountGroupMembers { get; set; }
        public virtual DbSet<AccountGroupSymbol> AccountGroupSymbols { get; set; }
        public virtual DbSet<AccountSettlementCurrency> AccountSettlementCurrencies { get; set; }
        public virtual DbSet<AccountType> AccountTypes { get; set; }
        public virtual DbSet<AccountTypeCurrency> AccountTypeCurrencies { get; set; }
        public virtual DbSet<AccountTypeExternalMap> AccountTypeExternalMaps { get; set; }
        public virtual DbSet<AccountVbloginMap> AccountVbloginMaps { get; set; }
        public virtual DbSet<ActiveAccountView> ActiveAccountViews { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ApplicationGroupContext> ApplicationGroupContexts { get; set; }
        public virtual DbSet<AuditTrail> AuditTrails { get; set; }
        public virtual DbSet<AuditTrailObject> AuditTrailObjects { get; set; }
        public virtual DbSet<Bk> Bks { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookBackup> BookBackups { get; set; }
        public virtual DbSet<BookDefaultHouse> BookDefaultHouses { get; set; }
        public virtual DbSet<BookGroup> BookGroups { get; set; }
        public virtual DbSet<BookReportView> BookReportViews { get; set; }
        public virtual DbSet<BorrowFee> BorrowFees { get; set; }
        public virtual DbSet<BorrowList> BorrowLists { get; set; }
        public virtual DbSet<BorrowRate> BorrowRates { get; set; }
        public virtual DbSet<BrokerAccount> BrokerAccounts { get; set; }
        public virtual DbSet<BrokerBottomAccount> BrokerBottomAccounts { get; set; }
        public virtual DbSet<BrokerBottomAccountProperty> BrokerBottomAccountProperties { get; set; }
        public virtual DbSet<BrokerDefaultHouse> BrokerDefaultHouses { get; set; }
        public virtual DbSet<BrokerTypeMain> BrokerTypeMains { get; set; }
        public virtual DbSet<CalculatedInterest> CalculatedInterests { get; set; }
        public virtual DbSet<CalculatedInterestDetail> CalculatedInterestDetails { get; set; }
        public virtual DbSet<CanadianAt> CanadianAts { get; set; }
        public virtual DbSet<CapTran> CapTrans { get; set; }
        public virtual DbSet<CapTransHist> CapTransHists { get; set; }
        public virtual DbSet<CapTransPerBookTotalView> CapTransPerBookTotalViews { get; set; }
        public virtual DbSet<CapTransReportView> CapTransReportViews { get; set; }
        public virtual DbSet<CapTransType> CapTransTypes { get; set; }
        public virtual DbSet<CashTran> CashTrans { get; set; }
        public virtual DbSet<CashTransPerBookTotalView> CashTransPerBookTotalViews { get; set; }
        public virtual DbSet<CashTransReportView> CashTransReportViews { get; set; }
        public virtual DbSet<CashTransType> CashTransTypes { get; set; }
        public virtual DbSet<Ccaccount> Ccaccounts { get; set; }
        public virtual DbSet<Ccbookkeeping> Ccbookkeepings { get; set; }
        public virtual DbSet<CcclientBalance> CcclientBalances { get; set; }
        public virtual DbSet<CcoptionsActivity> CcoptionsActivities { get; set; }
        public virtual DbSet<CcsecMaster> CcsecMasters { get; set; }
        public virtual DbSet<CcsecurityPosition> CcsecurityPositions { get; set; }
        public virtual DbSet<Cctrade> Cctrades { get; set; }
        public virtual DbSet<ClientIdentifierAccountOverride> ClientIdentifierAccountOverrides { get; set; }
        public virtual DbSet<ClientIdentifierAccountOverrideOld> ClientIdentifierAccountOverrideOlds { get; set; }
        public virtual DbSet<ClientIdentifierKey> ClientIdentifierKeys { get; set; }
        public virtual DbSet<ClientIdentifierKeysOld> ClientIdentifierKeysOlds { get; set; }
        public virtual DbSet<Commission> Commissions { get; set; }
        public virtual DbSet<CommissionGroup> CommissionGroups { get; set; }
        public virtual DbSet<CommissionGroupMember> CommissionGroupMembers { get; set; }
        public virtual DbSet<CommissionView> CommissionViews { get; set; }
        public virtual DbSet<CommissionView2> CommissionView2s { get; set; }
        public virtual DbSet<CommissionView20160919> CommissionView20160919s { get; set; }
        public virtual DbSet<CommissionView220160919> CommissionView220160919s { get; set; }
        public virtual DbSet<CommissionView2Test> CommissionView2Tests { get; set; }
        public virtual DbSet<CommissionViewBackup20130111> CommissionViewBackup20130111s { get; set; }
        public virtual DbSet<CommissionViewTest> CommissionViewTests { get; set; }
        public virtual DbSet<CommissiongroupmemberBackup20160717> CommissiongroupmemberBackup20160717s { get; set; }
        public virtual DbSet<CommissiongroupmemberBackup20160802> CommissiongroupmemberBackup20160802s { get; set; }
        public virtual DbSet<CommissiongroupmemberBackup20170621> CommissiongroupmemberBackup20170621s { get; set; }
        public virtual DbSet<CompanyProfile> CompanyProfiles { get; set; }
        public virtual DbSet<CompanyProfileCusipMatch> CompanyProfileCusipMatches { get; set; }
        public virtual DbSet<CompanyProfileOpt> CompanyProfileOpts { get; set; }
        public virtual DbSet<CompanyprofileBackup> CompanyprofileBackups { get; set; }
        public virtual DbSet<CompanyprofileBackup0314> CompanyprofileBackup0314s { get; set; }
        public virtual DbSet<CompanyprofileBackup20121213> CompanyprofileBackup20121213s { get; set; }
        public virtual DbSet<CompanyprofileoptBackup> CompanyprofileoptBackups { get; set; }
        public virtual DbSet<CurrencyList> CurrencyLists { get; set; }
        public virtual DbSet<CurrencySpread> CurrencySpreads { get; set; }
        public virtual DbSet<CurrencySpreadGroup> CurrencySpreadGroups { get; set; }
        public virtual DbSet<CurrencySpreadGroupMember> CurrencySpreadGroupMembers { get; set; }
        public virtual DbSet<CurrencySpreadView> CurrencySpreadViews { get; set; }
        public virtual DbSet<CurrentPosition> CurrentPositions { get; set; }
        public virtual DbSet<CurrentpositionBackup> CurrentpositionBackups { get; set; }
        public virtual DbSet<CusipFix> CusipFixes { get; set; }
        public virtual DbSet<Das1> Das1s { get; set; }
        public virtual DbSet<Das1AdpView> Das1AdpViews { get; set; }
        public virtual DbSet<Das4> Das4s { get; set; }
        public virtual DbSet<DebenturePayoutInfo> DebenturePayoutInfos { get; set; }
        public virtual DbSet<DebentureProfile> DebentureProfiles { get; set; }
        public virtual DbSet<Eodreport> Eodreports { get; set; }
        public virtual DbSet<EodreportHistory> EodreportHistories { get; set; }
        public virtual DbSet<EquityReportView> EquityReportViews { get; set; }
        public virtual DbSet<Etf> Etfs { get; set; }
        public virtual DbSet<EventHistoryTracker> EventHistoryTrackers { get; set; }
        public virtual DbSet<ExchangeHoliday> ExchangeHolidays { get; set; }
        public virtual DbSet<ExchangeList> ExchangeLists { get; set; }
        public virtual DbSet<ExchangeRoutingFee> ExchangeRoutingFees { get; set; }
        public virtual DbSet<Fxrate> Fxrates { get; set; }
        public virtual DbSet<FxrateHistory> FxrateHistories { get; set; }
        public virtual DbSet<GroupMember> GroupMembers { get; set; }
        public virtual DbSet<GroupUser> GroupUsers { get; set; }
        public virtual DbSet<ImportLog> ImportLogs { get; set; }
        public virtual DbSet<InsiderSecurity> InsiderSecurities { get; set; }
        public virtual DbSet<InterestRate> InterestRates { get; set; }
        public virtual DbSet<InterestRateReference> InterestRateReferences { get; set; }
        public virtual DbSet<InterestSpread> InterestSpreads { get; set; }
        public virtual DbSet<IntroducingParty> IntroducingParties { get; set; }
        public virtual DbSet<InvTransDescription> InvTransDescriptions { get; set; }
        public virtual DbSet<InvTransOptionalDatum> InvTransOptionalData { get; set; }
        public virtual DbSet<InvTransPerBookTotalView> InvTransPerBookTotalViews { get; set; }
        public virtual DbSet<InvTransaction> InvTransactions { get; set; }
        public virtual DbSet<InvTransactionConsolidatedView> InvTransactionConsolidatedViews { get; set; }
        public virtual DbSet<InvTransactionView> InvTransactionViews { get; set; }
        public virtual DbSet<InventoryManagerStatus> InventoryManagerStatuses { get; set; }
        public virtual DbSet<LockInfo> LockInfos { get; set; }
        public virtual DbSet<LockedRecord> LockedRecords { get; set; }
        public virtual DbSet<MemberAccount> MemberAccounts { get; set; }
        public virtual DbSet<MemberAccountDoNotUse> MemberAccountDoNotUses { get; set; }
        public virtual DbSet<MemberAccountHist> MemberAccountHists { get; set; }
        public virtual DbSet<NewAccountDefault> NewAccountDefaults { get; set; }
        public virtual DbSet<NewAccountExclusion> NewAccountExclusions { get; set; }
        public virtual DbSet<NewIssue> NewIssues { get; set; }
        public virtual DbSet<NewIssueAllocation> NewIssueAllocations { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<PermissionAssignment> PermissionAssignments { get; set; }
        public virtual DbSet<PermissionAssignmentsBob> PermissionAssignmentsBobs { get; set; }
        public virtual DbSet<PositionGroupReportView> PositionGroupReportViews { get; set; }
        public virtual DbSet<PositionJournalCompare> PositionJournalCompares { get; set; }
        public virtual DbSet<PositionReportView> PositionReportViews { get; set; }
        public virtual DbSet<PositionWithJournal> PositionWithJournals { get; set; }
        public virtual DbSet<PositionWithoutJournal> PositionWithoutJournals { get; set; }
        public virtual DbSet<PromotionalCommissionGroupMember> PromotionalCommissionGroupMembers { get; set; }
        public virtual DbSet<PromotionalCommissionView> PromotionalCommissionViews { get; set; }
        public virtual DbSet<QuoteHistory> QuoteHistories { get; set; }
        public virtual DbSet<QuoteInfo> QuoteInfos { get; set; }
        public virtual DbSet<QuoteInfoAllowableSmbListException> QuoteInfoAllowableSmbListExceptions { get; set; }
        public virtual DbSet<QuoteInfoAllowedSymbol> QuoteInfoAllowedSymbols { get; set; }
        public virtual DbSet<QuoteInfoAllowedSymbolsHistory> QuoteInfoAllowedSymbolsHistories { get; set; }
        public virtual DbSet<RegisteredAccountFee> RegisteredAccountFees { get; set; }
        public virtual DbSet<RegisteredAccountFxView> RegisteredAccountFxViews { get; set; }
        public virtual DbSet<RegisteredAccountFxposition> RegisteredAccountFxpositions { get; set; }
        public virtual DbSet<RegisteredAccountFxrate> RegisteredAccountFxrates { get; set; }
        public virtual DbSet<ResetCommissionGroupLog> ResetCommissionGroupLogs { get; set; }
        public virtual DbSet<RoutingInstruction> RoutingInstructions { get; set; }
        public virtual DbSet<Securable> Securables { get; set; }
        public virtual DbSet<SecurityGroup> SecurityGroups { get; set; }
        public virtual DbSet<SecurityType> SecurityTypes { get; set; }
        public virtual DbSet<SettlementInstruction> SettlementInstructions { get; set; }
        public virtual DbSet<Sl> Sls { get; set; }
        public virtual DbSet<StrategyType> StrategyTypes { get; set; }
        public virtual DbSet<TempTimeMarker> TempTimeMarkers { get; set; }
        public virtual DbSet<Tr> Trs { get; set; }
        public virtual DbSet<TradeDbBrokerMap> TradeDbBrokerMaps { get; set; }
        public virtual DbSet<TradeDbCurrencyMap> TradeDbCurrencyMaps { get; set; }
        public virtual DbSet<TradeDbExchangeMap> TradeDbExchangeMaps { get; set; }
        public virtual DbSet<TradeDbTraderBookMap> TradeDbTraderBookMaps { get; set; }
        public virtual DbSet<Trader> Traders { get; set; }
        public virtual DbSet<TraderAccessGroup> TraderAccessGroups { get; set; }
        public virtual DbSet<TraderAccount> TraderAccounts { get; set; }
        public virtual DbSet<TraderAccountDmagatewayView> TraderAccountDmagatewayViews { get; set; }
        public virtual DbSet<TraderAccountGroup> TraderAccountGroups { get; set; }
        public virtual DbSet<TraderAccountGroupMember> TraderAccountGroupMembers { get; set; }
        public virtual DbSet<TraderAccountView> TraderAccountViews { get; set; }
        public virtual DbSet<TraderAccountView2> TraderAccountView2s { get; set; }
        public virtual DbSet<TraderBookGroupAccess> TraderBookGroupAccesses { get; set; }
        public virtual DbSet<TransactionColumnOverride> TransactionColumnOverrides { get; set; }
        public virtual DbSet<TransactionFile> TransactionFiles { get; set; }
        public virtual DbSet<TransactionFileColumn> TransactionFileColumns { get; set; }
        public virtual DbSet<TransactionFileColumnDefault> TransactionFileColumnDefaults { get; set; }
        public virtual DbSet<TransactionFileDefinition> TransactionFileDefinitions { get; set; }
        public virtual DbSet<TransactionFileRecord> TransactionFileRecords { get; set; }
        public virtual DbSet<TransactionFileRecordType> TransactionFileRecordTypes { get; set; }
        public virtual DbSet<TransactionFileType> TransactionFileTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserAccessAccount> UserAccessAccounts { get; set; }
        public virtual DbSet<UserSecGroupLevel> UserSecGroupLevels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=mtDEVsqldb;Database=Inventory;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("CORPORATE\\apigida")
                .HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account", "dbo");

                entity.HasIndex(e => e.ShortName, "Constraint_ShortName")
                    .IsUnique();

                entity.HasIndex(e => new { e.AdpBranchCd, e.AdpAccountCd, e.AdpTypeAccountCd, e.AdpChk }, "IX_Account");

                entity.HasIndex(e => e.ShortName, "Index_ShortName");

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
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TsxStampId)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Account20160405>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Account_20160405", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountTypes)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AcctBrokerId).HasColumnName("acctBrokerID");

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

                entity.Property(e => e.BorRateId).HasColumnName("BorRateID");

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

                entity.Property(e => e.FxRateId).HasColumnName("FxRateID");

                entity.Property(e => e.IntRateId).HasColumnName("IntRateID");

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

                entity.Property(e => e.SecGroupId).HasColumnName("SecGroupID");

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TsxStampId)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AccountAdpView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("account_adp_view", "dbo");

                entity.Property(e => e.AccountCd)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("account_cd")
                    .IsFixedLength(true);

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountTypes)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AcctBrokerId).HasColumnName("acctBrokerID");

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

                entity.Property(e => e.AdpaccountCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ADPAccountCode");

                entity.Property(e => e.AdpgiveUpAccountCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ADPGiveUpAccountCode")
                    .IsFixedLength(true);

                entity.Property(e => e.BmoaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BMOAccountID");

                entity.Property(e => e.BorRateId).HasColumnName("BorRateID");

                entity.Property(e => e.BranchCd)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("branch_cd")
                    .IsFixedLength(true);

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

                entity.Property(e => e.FxRateId).HasColumnName("FxRateID");

                entity.Property(e => e.IntRateId).HasColumnName("IntRateID");

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

                entity.Property(e => e.SecGroupId).HasColumnName("SecGroupID");

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TsxStampId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TypeAccountCd)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("type_account_cd")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<AccountAdpView2>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("account_adp_view2", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountTypes)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AcctBrokerId).HasColumnName("acctBrokerID");

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

                entity.Property(e => e.AdpaccountCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ADPAccountCode");

                entity.Property(e => e.AdpgiveUpAccountCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ADPGiveUpAccountCode")
                    .IsFixedLength(true);

                entity.Property(e => e.BmoaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BMOAccountID");

                entity.Property(e => e.BorRateId).HasColumnName("BorRateID");

                entity.Property(e => e.CanaccordAccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CanaccordAccountID");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("currency");

                entity.Property(e => e.DefaultAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultBook)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FxRateId).HasColumnName("FxRateID");

                entity.Property(e => e.IntRateId).HasColumnName("IntRateID");

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

                entity.Property(e => e.SecGroupId).HasColumnName("SecGroupID");

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TsxStampId)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AccountAdpViewEx>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("account_adp_view_ex", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountTypes)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AcctBrokerId).HasColumnName("acctBrokerID");

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

                entity.Property(e => e.AdpaccountCode)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("ADPAccountCode");

                entity.Property(e => e.BmoaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BMOAccountID");

                entity.Property(e => e.BorRateId).HasColumnName("BorRateID");

                entity.Property(e => e.CanaccordAccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CanaccordAccountID");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("currency");

                entity.Property(e => e.DefaultAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultBook)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FxRateId).HasColumnName("FxRateID");

                entity.Property(e => e.IntRateId).HasColumnName("IntRateID");

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

                entity.Property(e => e.SecGroupId).HasColumnName("SecGroupID");

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TsxStampId)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AccountBackup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("account_backup", "dbo");

                entity.Property(e => e.AccountId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("Account_ID");

                entity.Property(e => e.AccountTypes)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AcctBrokerId).HasColumnName("acctBrokerID");

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

                entity.Property(e => e.BorRateId).HasColumnName("BorRateID");

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

                entity.Property(e => e.FxRateId).HasColumnName("FxRateID");

                entity.Property(e => e.IntRateId).HasColumnName("IntRateID");

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

                entity.Property(e => e.SecGroupId).HasColumnName("SecGroupID");

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TsxStampId)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AccountCurrencyDetail>(entity =>
            {
                entity.HasKey(e => new { e.Acctid, e.Currency });

                entity.ToTable("AccountCurrencyDetails", "dbo");

                entity.HasIndex(e => new { e.Acctid, e.Currency }, "IX_AccountCurrencyDetails_Unique")
                    .IsUnique();

                entity.Property(e => e.Acctid).HasColumnName("acctid");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("currency");

                entity.Property(e => e.AccountTypes)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.TraderId)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("TraderID");

                entity.Property(e => e.TsxStampId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TsxStampID");

                entity.HasOne(d => d.Acct)
                    .WithMany(p => p.AccountCurrencyDetails)
                    .HasForeignKey(d => d.Acctid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountCurrencyDetails_Account");
            });

            modelBuilder.Entity<AccountGroup>(entity =>
            {
                entity.HasKey(e => e.GroupId)
                    .HasName("PK_accountgroup");

                entity.ToTable("AccountGroup", "dbo");

                entity.HasIndex(e => e.GroupName, "IX_AccountGroup_GroupName")
                    .IsUnique();

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.BDebenture)
                    .HasColumnName("bDebenture")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BEquity)
                    .HasColumnName("bEquity")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BForEx)
                    .HasColumnName("bForEx")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BFuture)
                    .HasColumnName("bFuture")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BMleg)
                    .HasColumnName("bMLEG")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BNote)
                    .HasColumnName("bNote")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BOpt)
                    .HasColumnName("bOpt")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Enddate)
                    .HasColumnName("enddate")
                    .HasDefaultValueSql("(CONVERT([char](8),getdate(),(112)))");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.NumDebenture).HasDefaultValueSql("((0))");

                entity.Property(e => e.NumEquity).HasDefaultValueSql("((0))");

                entity.Property(e => e.NumForEx).HasDefaultValueSql("((0))");

                entity.Property(e => e.NumFuture).HasDefaultValueSql("((0))");

                entity.Property(e => e.NumMleg)
                    .HasColumnName("NumMLEG")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NumNote).HasDefaultValueSql("((0))");

                entity.Property(e => e.NumOpt).HasDefaultValueSql("((0))");

                entity.Property(e => e.NumTrades)
                    .HasColumnName("numTrades")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Startdate)
                    .HasColumnName("startdate")
                    .HasDefaultValueSql("(CONVERT([char](8),getdate(),(112)))");

                entity.Property(e => e.VbloginId)
                    .HasColumnName("VBLoginID")
                    .HasDefaultValueSql("((-10))");
            });

            modelBuilder.Entity<AccountGroupMember>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.AccountId });

                entity.ToTable("AccountGroupMember", "dbo");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountGroupMembers)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountGroupMember_Account");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.AccountGroupMembers)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_accountgroupmember_accountgroup");
            });

            modelBuilder.Entity<AccountGroupSymbol>(entity =>
            {
                entity.HasKey(e => new { e.Acctid, e.Usymbol });

                entity.ToTable("AccountGroupSymbol", "dbo");

                entity.Property(e => e.Acctid).HasColumnName("acctid");

                entity.Property(e => e.Usymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");

                entity.HasOne(d => d.Acct)
                    .WithMany(p => p.AccountGroupSymbols)
                    .HasForeignKey(d => d.Acctid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountGroupSymbol_Account");
            });

            modelBuilder.Entity<AccountSettlementCurrency>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.Currency });

                entity.ToTable("AccountSettlementCurrency", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountSettlementCurrencies)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountSettlementCurrency_Account");
            });

            modelBuilder.Entity<AccountType>(entity =>
            {
                entity.ToTable("AccountType", "dbo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AcctTypeId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("acctTypeID");

                entity.Property(e => e.AdpType).HasColumnName("adp_type");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PlanType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("plan_type");
            });

            modelBuilder.Entity<AccountTypeCurrency>(entity =>
            {
                entity.HasKey(e => new { e.AcctTypeId, e.Currency });

                entity.ToTable("AccountTypeCurrency", "dbo");

                entity.Property(e => e.AcctTypeId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("AcctTypeID");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AccountTypeExternalMap>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AccountTypeExternalMap", "dbo");

                entity.Property(e => e.AcctTypeId)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("acctTypeID");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ExAcctTypeId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ExAcctTypeID");
            });

            modelBuilder.Entity<AccountVbloginMap>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.ToTable("AccountVBLoginMap", "dbo");

                entity.Property(e => e.AccountId)
                    .ValueGeneratedNever()
                    .HasColumnName("Account_ID");

                entity.Property(e => e.VbLoginId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("vbLoginID");
            });

            modelBuilder.Entity<ActiveAccountView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ActiveAccount_View", "dbo");

                entity.Property(e => e.AccountCd)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("account_cd")
                    .IsFixedLength(true);

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountTypes)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AcctBrokerId).HasColumnName("acctBrokerID");

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

                entity.Property(e => e.AdpaccountCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ADPAccountCode");

                entity.Property(e => e.AdpgiveUpAccountCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ADPGiveUpAccountCode")
                    .IsFixedLength(true);

                entity.Property(e => e.BmoaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BMOAccountID");

                entity.Property(e => e.BorRateId).HasColumnName("BorRateID");

                entity.Property(e => e.BranchCd)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("branch_cd")
                    .IsFixedLength(true);

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

                entity.Property(e => e.FxRateId).HasColumnName("FxRateID");

                entity.Property(e => e.IntRateId).HasColumnName("IntRateID");

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

                entity.Property(e => e.SecGroupId).HasColumnName("SecGroupID");

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TsxStampId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TypeAccountCd)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("type_account_cd")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasKey(e => e.AppId);

                entity.ToTable("Application", "dbo");

                entity.Property(e => e.AppId)
                    .ValueGeneratedNever()
                    .HasColumnName("AppID");

                entity.Property(e => e.AppName)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ApplicationGroupContext>(entity =>
            {
                entity.HasKey(e => new { e.AppId, e.GroupId });

                entity.ToTable("ApplicationGroupContext", "dbo");

                entity.Property(e => e.AppId).HasColumnName("AppID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.HasOne(d => d.App)
                    .WithMany(p => p.ApplicationGroupContexts)
                    .HasForeignKey(d => d.AppId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationGroupContext_Application");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.ApplicationGroupContexts)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationGroupContext_Users");
            });

            modelBuilder.Entity<AuditTrail>(entity =>
            {
                entity.ToTable("AuditTrail", "dbo");

                entity.Property(e => e.AuditTrailId).HasColumnName("AuditTrailID");

                entity.Property(e => e.AuditKeyId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("AuditKeyID");

                entity.Property(e => e.AuditObjectId).HasColumnName("AuditObjectID");

                entity.Property(e => e.AuditObjectKey)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.DtStamp)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_stamp");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UserID");

                entity.Property(e => e.Xmlchanges)
                    .IsRequired()
                    .HasColumnType("xml")
                    .HasColumnName("XMLChanges");
            });

            modelBuilder.Entity<AuditTrailObject>(entity =>
            {
                entity.HasKey(e => e.AuditObjectId);

                entity.ToTable("AuditTrailObject", "dbo");

                entity.Property(e => e.AuditObjectId)
                    .ValueGeneratedNever()
                    .HasColumnName("AuditObjectID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Bk>(entity =>
            {
                entity.ToTable("BK", "dbo");

                entity.HasIndex(e => new { e.ProcessDateInt, e.AccountNumber }, "Index_DateAccount");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.InsertTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MarketCode)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionDesc)
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TransactionID");

                entity.Property(e => e.TransactionSubId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TransactionSubID");

                entity.Property(e => e.TransactionType)
                    .HasMaxLength(8)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book", "dbo");

                entity.HasIndex(e => e.ShortName, "Index_BookShortName")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "Index_Name")
                    .IsUnique();

                entity.HasIndex(e => new { e.ShortName, e.BookId }, "Index_SName_to_BookID")
                    .IsUnique()
                    .HasFillFactor((byte)80);

                entity.Property(e => e.BookId).HasColumnName("Book_ID");

                entity.Property(e => e.AccountNum)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DefMarket)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReportingCurrency)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.StrategyTypeNavigation)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.StrategyType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Book_StrategyType");
            });

            modelBuilder.Entity<BookBackup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("book_backup", "dbo");

                entity.Property(e => e.AccountNum)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.BookId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("Book_ID");

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

            modelBuilder.Entity<BookDefaultHouse>(entity =>
            {
                entity.HasKey(e => e.LeadId);

                entity.ToTable("BookDefaultHouse", "dbo");

                entity.Property(e => e.LeadId)
                    .ValueGeneratedNever()
                    .HasColumnName("Lead_ID");

                entity.Property(e => e.HouseId).HasColumnName("House_ID");

                entity.HasOne(d => d.Lead)
                    .WithOne(p => p.BookDefaultHouse)
                    .HasForeignKey<BookDefaultHouse>(d => d.LeadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DefaultHouse_Lead");
            });

            modelBuilder.Entity<BookGroup>(entity =>
            {
                entity.HasKey(e => e.GroupId)
                    .HasName("PK_Group");

                entity.ToTable("BookGroup", "dbo");

                entity.HasIndex(e => e.Name, "IX_GroupName")
                    .IsUnique();

                entity.Property(e => e.GroupId).HasColumnName("Group_ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BookReportView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("BookReport_view", "dbo");

                entity.Property(e => e.AccountNum)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.BookId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("Book_ID");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DefMarket)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Groups)
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReportingCurrency)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BorrowFee>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.ToTable("BorrowFee", "dbo");

                entity.Property(e => e.AccountId)
                    .ValueGeneratedNever()
                    .HasColumnName("Account_ID");
            });

            modelBuilder.Entity<BorrowList>(entity =>
            {
                entity.HasKey(e => new { e.Location, e.Account, e.Symbol, e.FileDateInt });

                entity.ToTable("BorrowList", "dbo");

                entity.HasIndex(e => new { e.FileDateInt, e.Symbol, e.Location, e.Account }, "IX_BorrowList")
                    .IsUnique();

                entity.Property(e => e.Location)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Account)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.AddBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AmountUsed).HasDefaultValueSql("((0))");

                entity.Property(e => e.Source)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BorrowRate>(entity =>
            {
                entity.ToTable("BorrowRate", "dbo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BrokerAccount>(entity =>
            {
                entity.ToTable("BrokerAccount", "dbo");

                entity.HasIndex(e => e.AccountName, "IX_BrokerAccount")
                    .IsUnique();

                entity.HasIndex(e => e.AccountName, "IX_BrokerAccount_1")
                    .IsUnique();

                entity.HasIndex(e => e.ShortAccountName, "IX_ShortAccountName")
                    .IsUnique();

                entity.Property(e => e.BrokerAccountId).HasColumnName("BrokerAccount_ID");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BrokerName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BrokerType).HasDefaultValueSql("((1))");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ShortAccountName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BrokerBottomAccount>(entity =>
            {
                entity.HasKey(e => new { e.BrokerName, e.SecurityType, e.ExchangeCode });

                entity.ToTable("BrokerBottomAccount", "dbo");

                entity.Property(e => e.BrokerName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SecurityType)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.ExchangeCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.AccountCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AdpaccountCode)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("ADPAccountCode");
            });

            modelBuilder.Entity<BrokerBottomAccountProperty>(entity =>
            {
                entity.HasKey(e => e.AccountCode);

                entity.ToTable("BrokerBottomAccountProperties", "dbo");

                entity.Property(e => e.AccountCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Bsecfee)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("BSECFee");
            });

            modelBuilder.Entity<BrokerDefaultHouse>(entity =>
            {
                entity.HasKey(e => new { e.BrokerAccountId, e.HouseId });

                entity.ToTable("BrokerDefaultHouse", "dbo");

                entity.Property(e => e.BrokerAccountId).HasColumnName("BrokerAccount_ID");

                entity.Property(e => e.HouseId).HasColumnName("House_ID");

                entity.HasOne(d => d.BrokerAccount)
                    .WithMany(p => p.BrokerDefaultHouseBrokerAccounts)
                    .HasForeignKey(d => d.BrokerAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DefaultHouse_Broker_NewLead");

                entity.HasOne(d => d.House)
                    .WithMany(p => p.BrokerDefaultHouseHouses)
                    .HasForeignKey(d => d.HouseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DefaultHouse_Broker_House");
            });

            modelBuilder.Entity<BrokerTypeMain>(entity =>
            {
                entity.HasKey(e => e.BrokerTypeId);

                entity.ToTable("BrokerTypeMain", "dbo");

                entity.HasIndex(e => e.BrokerTypeId, "UQ__BrokerTypeMain__41D8BC2C")
                    .IsUnique();

                entity.Property(e => e.BrokerTypeId)
                    .HasMaxLength(2)
                    .HasColumnName("BrokerType_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.BrokerTypeDesc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CalculatedInterest>(entity =>
            {
                entity.HasKey(e => new { e.DateInt, e.AccountId, e.Currency });

                entity.ToTable("CalculatedInterest", "dbo");

                entity.HasIndex(e => e.Id, "Index_ID_Unique")
                    .IsUnique();

                entity.Property(e => e.AccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("AccountID");

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CalculatedInterest1).HasColumnName("CalculatedInterest");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.InterestCode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<CalculatedInterestDetail>(entity =>
            {
                entity.ToTable("CalculatedInterestDetail", "dbo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RootId).HasColumnName("RootID");

                entity.Property(e => e.UnderlyingInterestCode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Root)
                    .WithMany(p => p.CalculatedInterestDetails)
                    .HasPrincipalKey(p => p.Id)
                    .HasForeignKey(d => d.RootId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CalculatedInterestDetail_CalculatedInterest");
            });

            modelBuilder.Entity<CanadianAt>(entity =>
            {
                entity.HasKey(e => new { e.ExchangeCode, e.ListedExchangeCode, e.TicketExchangeCode });

                entity.ToTable("CanadianATS", "dbo");

                entity.Property(e => e.ExchangeCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ListedExchangeCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TicketExchangeCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CapTran>(entity =>
            {
                entity.HasKey(e => e.CapTransId);

                entity.ToTable("CapTrans", "dbo");

                entity.Property(e => e.CapTransId).HasColumnName("CapTrans_ID");

                entity.Property(e => e.BookId).HasColumnName("Book_ID");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.CxlOrCorrectDate).HasColumnType("datetime");

                entity.Property(e => e.CxlorCorrect)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Fxrate).HasColumnName("FXRate");

                entity.Property(e => e.RefCapTransId).HasColumnName("RefCapTrans_ID");

                entity.Property(e => e.TransDateInt).HasDefaultValueSql("((0))");

                entity.Property(e => e.TransType)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Usymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.CapTrans)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CapTrans_Book");

                entity.HasOne(d => d.TransTypeNavigation)
                    .WithMany(p => p.CapTrans)
                    .HasForeignKey(d => d.TransType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CapTrans_CapTransType");

                entity.HasOne(d => d.UsymbolNavigation)
                    .WithMany(p => p.CapTrans)
                    .HasForeignKey(d => d.Usymbol)
                    .HasConstraintName("FK_CapTrans_CompanyProfile");
            });

            modelBuilder.Entity<CapTransHist>(entity =>
            {
                entity.ToTable("CapTransHist", "dbo");

                entity.Property(e => e.CapTransHistId).HasColumnName("CapTransHist_ID");

                entity.Property(e => e.BookId).HasColumnName("Book_ID");

                entity.Property(e => e.Ltd)
                    .HasColumnType("money")
                    .HasColumnName("LTD");

                entity.Property(e => e.Mtd)
                    .HasColumnType("money")
                    .HasColumnName("MTD");

                entity.Property(e => e.Today).HasColumnType("money");

                entity.Property(e => e.TransType)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.Property(e => e.Yesterday).HasColumnType("money");

                entity.Property(e => e.Ytd)
                    .HasColumnType("money")
                    .HasColumnName("YTD");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.CapTransHists)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CapTransHist_Book");
            });

            modelBuilder.Entity<CapTransPerBookTotalView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CapTransPerBookTotal_view", "dbo");

                entity.Property(e => e.BookId).HasColumnName("Book_ID");
            });

            modelBuilder.Entity<CapTransReportView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CapTransReport_view", "dbo");

                entity.Property(e => e.BookCurrency)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.BookId).HasColumnName("Book_ID");

                entity.Property(e => e.BookName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ExDivDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.RecordDate).HasColumnType("datetime");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TransDate).HasColumnType("datetime");

                entity.Property(e => e.TransType)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.TransTypeString)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<CapTransType>(entity =>
            {
                entity.HasKey(e => e.TransType)
                    .HasName("PK_CapTransTypes");

                entity.ToTable("CapTransType", "dbo");

                entity.Property(e => e.TransType)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CashTran>(entity =>
            {
                entity.HasKey(e => e.CashTransId);

                entity.ToTable("CashTrans", "dbo");

                entity.Property(e => e.CashTransId).HasColumnName("CashTrans_ID");

                entity.Property(e => e.BookId).HasColumnName("Book_ID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CxlOrCorrect)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CxlOrCorrectDate).HasColumnType("datetime");

                entity.Property(e => e.RefCashTransId).HasColumnName("RefCashTrans_ID");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.CashTrans)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CashTrans_Book");

                entity.HasOne(d => d.TransTypeNavigation)
                    .WithMany(p => p.CashTrans)
                    .HasForeignKey(d => d.TransType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CashTrans_CashTransType");
            });

            modelBuilder.Entity<CashTransPerBookTotalView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CashTransPerBookTotal_view", "dbo");

                entity.Property(e => e.BookId).HasColumnName("Book_ID");
            });

            modelBuilder.Entity<CashTransReportView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CashTransReport_view", "dbo");

                entity.Property(e => e.BookId).HasColumnName("Book_ID");

                entity.Property(e => e.BookName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comment)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SettlDate).HasColumnType("datetime");

                entity.Property(e => e.TransDate).HasColumnType("datetime");

                entity.Property(e => e.TransTypeString)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CashTransType>(entity =>
            {
                entity.HasKey(e => e.TransType);

                entity.ToTable("CashTransType", "dbo");

                entity.HasIndex(e => e.Description, "IX_CashTransTypeDesc")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ccaccount>(entity =>
            {
                entity.HasKey(e => new { e.FileDateInt, e.Id });

                entity.ToTable("CCAccount", "dbo");

                entity.Property(e => e.FileDateInt).HasColumnName("fileDateInt");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.AcctClass)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.AcctFund)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.AcctLang)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.AcctNum)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.AcctShrtname)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.AcctSin)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("AcctSIN");

                entity.Property(e => e.AcctSubType)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.AcctType)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.AddrLvl)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("Addr_lvl");

                entity.Property(e => e.AddrNum)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("Addr_Num");

                entity.Property(e => e.Address1)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Address3)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Address4)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Address5)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Address6)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Address7)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Address8)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Approvedby)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BenBdate).HasColumnName("BenBDate");

                entity.Property(e => e.BenSin)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("BenSIN");

                entity.Property(e => e.Beneficiary)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.BrnNum)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ClientNum)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.CommType)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Credintcd)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Debintcd)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.EConfirm)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("eConfirm");

                entity.Property(e => e.EStatement)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("eStatement");

                entity.Property(e => e.Employee)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.InternalClientNumber)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LockInInd)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LockJourisdiction)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Ni54a)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("NI54a");

                entity.Property(e => e.Ni54b)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("NI54b");

                entity.Property(e => e.Ni54c)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("NI54c");

                entity.Property(e => e.Ni54d)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("NI54d");

                entity.Property(e => e.NonResCode)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.OptCode)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneOther)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneOther1)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneOther2)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneOther3)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PortType)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.PostCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RecipientType)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ResCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Rrcode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("RRCode");

                entity.Property(e => e.SpousalInd)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.SpouseJointBdate).HasColumnName("SpouseJointBDate");

                entity.Property(e => e.SpouseJointName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.SpouseJointSin)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("SpouseJointSIN");

                entity.Property(e => e.Stat)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.StruckAdrInd)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.TradesOk)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TradesOK");
            });

            modelBuilder.Entity<Ccbookkeeping>(entity =>
            {
                entity.HasKey(e => new { e.FileDateInt, e.Id });

                entity.ToTable("CCBookkeeping", "dbo");

                entity.Property(e => e.FileDateInt).HasColumnName("fileDateInt");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.AccruedInt).HasColumnType("decimal(13, 5)");

                entity.Property(e => e.AcctNum)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Amount).HasColumnType("decimal(15, 5)");

                entity.Property(e => e.Cost).HasColumnType("decimal(14, 5)");

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Cusip)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.CustCommission).HasColumnType("decimal(14, 5)");

                entity.Property(e => e.Description)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.FillPrice).HasColumnType("decimal(12, 6)");

                entity.Property(e => e.GrossTradeAmt).HasColumnType("decimal(14, 5)");

                entity.Property(e => e.Interface)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.JournalRefNum)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Pacid)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PACID");

                entity.Property(e => e.ProcDate)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Qty).HasColumnType("decimal(16, 5)");

                entity.Property(e => e.Rrcode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("RRCode");

                entity.Property(e => e.Rrcommission)
                    .HasColumnType("decimal(14, 5)")
                    .HasColumnName("RRCommission");

                entity.Property(e => e.SecNum)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.SettleDate)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.TradeDate)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.TradeNumber)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.TransCode)
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CcclientBalance>(entity =>
            {
                entity.HasKey(e => new { e.FileDateInt, e.Id });

                entity.ToTable("CCClientBalance", "dbo");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.AcctNum)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.SdcashBalance)
                    .HasColumnType("decimal(15, 5)")
                    .HasColumnName("SDCashBalance");

                entity.Property(e => e.Sdequity)
                    .HasColumnType("decimal(15, 5)")
                    .HasColumnName("SDEquity");

                entity.Property(e => e.SdmarketValue)
                    .HasColumnType("decimal(15, 5)")
                    .HasColumnName("SDMarketValue");

                entity.Property(e => e.SdshortPosMv)
                    .HasColumnType("decimal(15, 5)")
                    .HasColumnName("SDShortPosMV");

                entity.Property(e => e.SfkPosMv)
                    .HasColumnType("decimal(15, 5)")
                    .HasColumnName("SfkPosMV");

                entity.Property(e => e.TdcashBalance)
                    .HasColumnType("decimal(15, 5)")
                    .HasColumnName("TDCashBalance");

                entity.Property(e => e.Tdequity)
                    .HasColumnType("decimal(15, 5)")
                    .HasColumnName("TDEquity");

                entity.Property(e => e.TdmarketValue)
                    .HasColumnType("decimal(15, 5)")
                    .HasColumnName("TDMarketValue");

                entity.Property(e => e.TdshortPosMv)
                    .HasColumnType("decimal(15, 5)")
                    .HasColumnName("TDShortPosMV");

                entity.Property(e => e.TtlBookValue).HasColumnType("decimal(15, 5)");

                entity.Property(e => e.TtlLoanValue).HasColumnType("decimal(15, 5)");

                entity.Property(e => e.TtlMonthEndMv)
                    .HasColumnType("decimal(15, 5)")
                    .HasColumnName("TtlMonthEndMV");
            });

            modelBuilder.Entity<CcoptionsActivity>(entity =>
            {
                entity.HasKey(e => new { e.FileDateInt, e.Id });

                entity.ToTable("CCOptionsActivity", "dbo");

                entity.Property(e => e.FileDateInt).HasColumnName("fileDateInt");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Acct)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("acct");

                entity.Property(e => e.Amt)
                    .HasColumnType("decimal(15, 5)")
                    .HasColumnName("amt");

                entity.Property(e => e.Cusip)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("cusip");

                entity.Property(e => e.ExpDt)
                    .HasColumnType("datetime")
                    .HasColumnName("exp_dt");

                entity.Property(e => e.FundType)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("fund_type");

                entity.Property(e => e.MktCode)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("mkt_code");

                entity.Property(e => e.ProcDt)
                    .HasColumnType("datetime")
                    .HasColumnName("proc_dt");

                entity.Property(e => e.Qty).HasColumnName("qty");

                entity.Property(e => e.ShortName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("short_name");

                entity.Property(e => e.StlDt)
                    .HasColumnType("datetime")
                    .HasColumnName("stl_dt");

                entity.Property(e => e.StrikePrc)
                    .HasColumnType("decimal(15, 5)")
                    .HasColumnName("strike_prc");

                entity.Property(e => e.TranCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("tran_code");

                entity.Property(e => e.TrdDt)
                    .HasColumnType("datetime")
                    .HasColumnName("trd_dt");
            });

            modelBuilder.Entity<CcsecMaster>(entity =>
            {
                entity.HasKey(e => e.CcsecId)
                    .HasName("PK_CCSecMaster_1");

                entity.ToTable("CCSecMaster", "dbo");

                entity.Property(e => e.CcsecId).HasColumnName("ccsec_id");

                entity.Property(e => e.AlterMktCode)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.AlterMktPrice).HasColumnType("decimal(11, 5)");

                entity.Property(e => e.Bbselig)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BBSElig");

                entity.Property(e => e.Cdselig)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CDSElig");

                entity.Property(e => e.CertNumber)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Cnselig)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CNSElig");

                entity.Property(e => e.Code1)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.CorpInd)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Coupons)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Csselig)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CSSElig");

                entity.Property(e => e.CurrentDivRate).HasColumnType("decimal(8, 5)");

                entity.Property(e => e.CurrentPayableDate)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CurrentRecordDate)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CusipNum)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.DcsEcselig)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DCS_ECSElig");

                entity.Property(e => e.DiscountNote)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.DividendCurrency)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.DividendYield).HasColumnType("decimal(9, 5)");

                entity.Property(e => e.Dppelig)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DPPELig");

                entity.Property(e => e.EuroElig)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ExDividendDate)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.FileDateInt).HasColumnName("fileDateInt");

                entity.Property(e => e.Filler1)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Filler2)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.FirstCouponDate)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.FixedRate)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Fraction).HasColumnType("decimal(11, 5)");

                entity.Property(e => e.FundInd)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Industry)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.InterestRate).HasColumnType("decimal(8, 5)");

                entity.Property(e => e.InterestRate2).HasColumnType("decimal(8, 5)");

                entity.Property(e => e.InterestRate3).HasColumnType("decimal(8, 5)");

                entity.Property(e => e.LastTrdPrice).HasColumnType("decimal(11, 5)");

                entity.Property(e => e.Market)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Mdwelig)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("MDWElig");

                entity.Property(e => e.Nidselig)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("NIDSElig");

                entity.Property(e => e.OptionType)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Oscexempt)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("OSCexempt");

                entity.Property(e => e.PaymentFreq)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Qscexempt)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("QSCExempt");

                entity.Property(e => e.Qsspelig)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("QSSPElig");

                entity.Property(e => e.Rrspelignd)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("RRSPElignd");

                entity.Property(e => e.SecDescr1)
                    .HasMaxLength(29)
                    .IsUnicode(false);

                entity.Property(e => e.SecDescr2)
                    .HasMaxLength(58)
                    .IsUnicode(false);

                entity.Property(e => e.SecNum)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Sector)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.StrikePrice).HasColumnType("decimal(11, 5)");

                entity.Property(e => e.SubSector)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol2)
                    .HasMaxLength(17)
                    .IsUnicode(false);

                entity.Property(e => e.TradesOption)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.UnderlyingCusip)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("UnderlyingCUSIP");

                entity.Property(e => e.Vseelig)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("VSEElig");
            });

            modelBuilder.Entity<CcsecurityPosition>(entity =>
            {
                entity.HasKey(e => new { e.FileDateInt, e.Id });

                entity.ToTable("CCSecurityPositions", "dbo");

                entity.Property(e => e.FileDateInt).HasColumnName("fileDateInt");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.AcctNum)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.BookVal).HasColumnType("decimal(16, 5)");

                entity.Property(e => e.CurrencyInd)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CurrentQty).HasColumnType("decimal(15, 5)");

                entity.Property(e => e.Cusip)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.FundAcctNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.LoanRate).HasColumnType("decimal(12, 5)");

                entity.Property(e => e.MktPrice).HasColumnType("decimal(14, 5)");

                entity.Property(e => e.PndQty).HasColumnType("decimal(15, 5)");

                entity.Property(e => e.SafeQty).HasColumnType("decimal(15, 5)");

                entity.Property(e => e.SdloanVal)
                    .HasColumnType("decimal(16, 5)")
                    .HasColumnName("SDLoanVal");

                entity.Property(e => e.SdmktVal)
                    .HasColumnType("decimal(16, 5)")
                    .HasColumnName("SDMktVal");

                entity.Property(e => e.SecNum)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.TdloanVal)
                    .HasColumnType("decimal(16, 5)")
                    .HasColumnName("TDLoanVal");

                entity.Property(e => e.TdmktVal)
                    .HasColumnType("decimal(16, 5)")
                    .HasColumnName("TDMktVal");
            });

            modelBuilder.Entity<Cctrade>(entity =>
            {
                entity.HasKey(e => new { e.FileDateInt, e.Id });

                entity.ToTable("CCTrade", "dbo");

                entity.Property(e => e.FileDateInt).HasColumnName("fileDateInt");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.AcctNum)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.BrnCd)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.BuySellInd)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.CancelInd)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CommAmount).HasColumnType("decimal(14, 5)");

                entity.Property(e => e.CommPrefigInd)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.CommissionRr)
                    .HasColumnType("decimal(14, 5)")
                    .HasColumnName("Commission_RR");

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CusipNum)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.Dsccharge)
                    .HasColumnType("decimal(9, 5)")
                    .HasColumnName("DSCCharge");

                entity.Property(e => e.ExcAmount).HasColumnType("decimal(14, 5)");

                entity.Property(e => e.Forex).HasColumnType("decimal(9, 6)");

                entity.Property(e => e.GrossAmount).HasColumnType("decimal(14, 5)");

                entity.Property(e => e.IntAmount).HasColumnType("decimal(13, 5)");

                entity.Property(e => e.Mkt)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.NetAmount).HasColumnType("decimal(13, 5)");

                entity.Property(e => e.NumMf)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("NumMF");

                entity.Property(e => e.OrderSource)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(12, 5)");

                entity.Property(e => e.Qty).HasColumnType("decimal(16, 5)");

                entity.Property(e => e.Rrcode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("RRCode");

                entity.Property(e => e.SecNum)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Sfamount)
                    .HasColumnType("decimal(8, 5)")
                    .HasColumnName("SFAmount");

                entity.Property(e => e.Taxes).HasColumnType("decimal(8, 5)");
            });

            modelBuilder.Entity<ClientIdentifierAccountOverride>(entity =>
            {
                entity.HasKey(e => e.Acctid)
                    .HasName("PK_ClientIdentifierAccountOverride_1");

                entity.ToTable("ClientIdentifierAccountOverride", "dbo");

                entity.Property(e => e.Acctid)
                    .ValueGeneratedNever()
                    .HasColumnName("acctid");

                entity.Property(e => e.DefaultAlgorithmId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DefaultAlgorithmID");

                entity.Property(e => e.DefaultBrokerLei)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DefaultBrokerLEI");

                entity.Property(e => e.DefaultCustomerAccount)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultCustomerLei)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DefaultCustomerLEI");

                entity.Property(e => e.DefaultOrderOrigination)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultRoutingArrangementIndicator)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideAlgorithmId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("OverrideAlgorithmID");

                entity.Property(e => e.OverrideBrokerLei)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("OverrideBrokerLEI");

                entity.Property(e => e.OverrideCustomerAccount)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideCustomerLei)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("OverrideCustomerLEI");

                entity.Property(e => e.OverrideOrderOrigination)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideRoutingArrangementIndicator)
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ClientIdentifierAccountOverrideOld>(entity =>
            {
                entity.HasKey(e => new { e.Acctid, e.Currency });

                entity.ToTable("ClientIdentifierAccountOverride_old", "dbo");

                entity.Property(e => e.Acctid).HasColumnName("acctid");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('CAD')");

                entity.Property(e => e.ApplyBrokerLeiId).HasColumnName("ApplyBrokerLEI_ID");

                entity.Property(e => e.ApplyCustomerLeiId).HasColumnName("ApplyCustomerLEI_ID");

                entity.Property(e => e.OverrideAlgorithmId)
                    .HasMaxLength(21)
                    .IsUnicode(false)
                    .HasColumnName("OverrideAlgorithmID");

                entity.Property(e => e.OverrideOrderOrigination)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideRoutingArrangementIndicator)
                    .HasMaxLength(8)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ClientIdentifierKey>(entity =>
            {
                entity.HasKey(e => e.LeiId)
                    .HasName("PK_ClientIdentifier_keys");

                entity.ToTable("ClientIdentifierKeys", "dbo");

                entity.Property(e => e.LeiId)
                    .ValueGeneratedNever()
                    .HasColumnName("LEI_ID");

                entity.Property(e => e.KeyEndDate)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.KeyStartDate)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.LeiencrptionKey)
                    .HasMaxLength(24)
                    .IsUnicode(false)
                    .HasColumnName("LEIEncrptionKey");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UniqueDealerId)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("UniqueDealerID");
            });

            modelBuilder.Entity<ClientIdentifierKeysOld>(entity =>
            {
                entity.HasKey(e => e.LeiId)
                    .HasName("PK_ClientIdentifier_keys_old");

                entity.ToTable("ClientIdentifierKeys_old", "dbo");

                entity.Property(e => e.LeiId)
                    .ValueGeneratedNever()
                    .HasColumnName("LEI_ID");

                entity.Property(e => e.BrokerLeikey)
                    .HasMaxLength(53)
                    .IsUnicode(false)
                    .HasColumnName("BrokerLEIKey");

                entity.Property(e => e.CustomerLeikey)
                    .HasMaxLength(53)
                    .IsUnicode(false)
                    .HasColumnName("CustomerLEIKey");

                entity.Property(e => e.KeyEndDate)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.KeyStartDate)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.LeiType)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("LEI_Type");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UniqueDealerId)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("UniqueDealerID");
            });

            modelBuilder.Entity<Commission>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.TradeSource, e.Exchange, e.SecurityType, e.TradeSessionId, e.PriceRangeMin, e.PriceRangeMax });

                entity.ToTable("Commission", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.TradeSource)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('tm')");

                entity.Property(e => e.Exchange)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.SecurityType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.TradeSessionId).HasColumnName("TradeSessionID");

                entity.Property(e => e.PriceRangeMin).HasDefaultValueSql("((-9999999))");

                entity.Property(e => e.PriceRangeMax).HasDefaultValueSql("((9999999))");

                entity.Property(e => e.AddRegulatoryExchangeFeeBeforeOrAfterMinMax)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('b')")
                    .IsFixedLength(true);

                entity.Property(e => e.CommissionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CommissionID");
            });

            modelBuilder.Entity<CommissionGroup>(entity =>
            {
                entity.ToTable("CommissionGroup", "dbo");

                entity.HasIndex(e => e.GroupName, "CommissionGroup_UniqueIndex")
                    .IsUnique();

                entity.Property(e => e.CommissionGroupId).HasColumnName("CommissionGroup_ID");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CommissionGroupMember>(entity =>
            {
                entity.HasKey(e => new { e.CommissionGroupId, e.AccountId });

                entity.ToTable("CommissionGroupMember", "dbo");

                entity.HasIndex(e => e.AccountId, "Index_AccountID");

                entity.HasIndex(e => e.AccountId, "Index_CommissionGroupMember_AccountID");

                entity.Property(e => e.CommissionGroupId).HasColumnName("CommissionGroup_ID");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.CommissionGroupMembers)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommissionGroupMember_Account");

                entity.HasOne(d => d.CommissionGroup)
                    .WithMany(p => p.CommissionGroupMembers)
                    .HasForeignKey(d => d.CommissionGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommissionGroupMember_CommissionGroup");
            });

            modelBuilder.Entity<CommissionView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("commission_view", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountTypes)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Accttypeid)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("accttypeid");

                entity.Property(e => e.ActId).HasColumnName("ActID");

                entity.Property(e => e.AddRegulatoryExchangeFeeBeforeOrAfterMinMax)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BmoaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BMOAccountID");

                entity.Property(e => e.Bpsaccount)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("BPSAccount");

                entity.Property(e => e.CommissionId).HasColumnName("CommissionID");

                entity.Property(e => e.DefaultAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultBook)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultRoutingInstruction)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Exchange)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideRoutingInstruction)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.PensonAccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonAccountID");

                entity.Property(e => e.PensonUsaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonUSAccountID");

                entity.Property(e => e.SecurityType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TradeSessionId).HasColumnName("TradeSessionID");

                entity.Property(e => e.TradeSource)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TsxStampId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TsxStampID");

                entity.Property(e => e.Typ)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.TypeDescription)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CommissionView2>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("commission_view2", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountTypes)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Accttypeid)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("accttypeid");

                entity.Property(e => e.ActId).HasColumnName("ActID");

                entity.Property(e => e.AddRegulatoryExchangeFeeBeforeOrAfterMinMax)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BmoaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BMOAccountID");

                entity.Property(e => e.Bpsaccount)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("BPSAccount");

                entity.Property(e => e.CommissionId).HasColumnName("CommissionID");

                entity.Property(e => e.DefaultAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultBook)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultRoutingInstruction)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Exchange)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideRoutingInstruction)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.PensonAccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonAccountID");

                entity.Property(e => e.PensonUsaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonUSAccountID");

                entity.Property(e => e.SecurityType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TradeSessionId).HasColumnName("TradeSessionID");

                entity.Property(e => e.TradeSource)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TsxStampId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TsxStampID");

                entity.Property(e => e.Typ)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.TypeDescription)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CommissionView20160919>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("commission_view_20160919", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountTypes)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Accttypeid)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("accttypeid");

                entity.Property(e => e.ActId).HasColumnName("ActID");

                entity.Property(e => e.AddRegulatoryExchangeFeeBeforeOrAfterMinMax)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BmoaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BMOAccountID");

                entity.Property(e => e.Bpsaccount)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("BPSAccount");

                entity.Property(e => e.CommissionId).HasColumnName("CommissionID");

                entity.Property(e => e.DefaultAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultBook)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultRoutingInstruction)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Exchange)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideRoutingInstruction)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.PensonAccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonAccountID");

                entity.Property(e => e.PensonUsaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonUSAccountID");

                entity.Property(e => e.SecurityType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TradeSessionId).HasColumnName("TradeSessionID");

                entity.Property(e => e.TradeSource)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TsxStampId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TsxStampID");

                entity.Property(e => e.Typ)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.TypeDescription)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CommissionView220160919>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("commission_view2_20160919", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountTypes)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Accttypeid)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("accttypeid");

                entity.Property(e => e.ActId).HasColumnName("ActID");

                entity.Property(e => e.AddRegulatoryExchangeFeeBeforeOrAfterMinMax)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BmoaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BMOAccountID");

                entity.Property(e => e.Bpsaccount)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("BPSAccount");

                entity.Property(e => e.CommissionId).HasColumnName("CommissionID");

                entity.Property(e => e.DefaultAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultBook)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultRoutingInstruction)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Exchange)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideRoutingInstruction)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.PensonAccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonAccountID");

                entity.Property(e => e.PensonUsaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonUSAccountID");

                entity.Property(e => e.SecurityType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TradeSessionId).HasColumnName("TradeSessionID");

                entity.Property(e => e.TradeSource)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TsxStampId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TsxStampID");

                entity.Property(e => e.Typ)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.TypeDescription)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CommissionView2Test>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("commission_view2_test", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountTypes)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Accttypeid)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("accttypeid");

                entity.Property(e => e.ActId).HasColumnName("ActID");

                entity.Property(e => e.AddRegulatoryExchangeFeeBeforeOrAfterMinMax)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BmoaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BMOAccountID");

                entity.Property(e => e.Bpsaccount)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("BPSAccount");

                entity.Property(e => e.CommissionId).HasColumnName("CommissionID");

                entity.Property(e => e.DefaultAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultBook)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultRoutingInstruction)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Exchange)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideRoutingInstruction)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.PensonAccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonAccountID");

                entity.Property(e => e.PensonUsaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonUSAccountID");

                entity.Property(e => e.SecurityType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TradeSessionId).HasColumnName("TradeSessionID");

                entity.Property(e => e.TradeSource)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TsxStampId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TsxStampID");

                entity.Property(e => e.Typ)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.TypeDescription)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CommissionViewBackup20130111>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("commission_view_backup_20130111", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountTypes)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Accttypeid)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("accttypeid");

                entity.Property(e => e.ActId).HasColumnName("ActID");

                entity.Property(e => e.AddRegulatoryExchangeFeeBeforeOrAfterMinMax)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BmoaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BMOAccountID");

                entity.Property(e => e.Bpsaccount)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("BPSAccount");

                entity.Property(e => e.CommissionId).HasColumnName("CommissionID");

                entity.Property(e => e.DefaultAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultBook)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultRoutingInstruction)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Exchange)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideRoutingInstruction)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.PensonAccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonAccountID");

                entity.Property(e => e.PensonUsaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonUSAccountID");

                entity.Property(e => e.SecurityType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TradeSessionId).HasColumnName("TradeSessionID");

                entity.Property(e => e.TradeSource)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TsxStampId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TsxStampID");

                entity.Property(e => e.Typ)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.TypeDescription)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CommissionViewTest>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("commission_view_test", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountTypes)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Accttypeid)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("accttypeid");

                entity.Property(e => e.ActId).HasColumnName("ActID");

                entity.Property(e => e.AddRegulatoryExchangeFeeBeforeOrAfterMinMax)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BmoaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BMOAccountID");

                entity.Property(e => e.Bpsaccount)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("BPSAccount");

                entity.Property(e => e.CommissionId).HasColumnName("CommissionID");

                entity.Property(e => e.DefaultAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultBook)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultRoutingInstruction)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Exchange)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideRoutingInstruction)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.PensonAccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonAccountID");

                entity.Property(e => e.PensonUsaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonUSAccountID");

                entity.Property(e => e.SecurityType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TradeSessionId).HasColumnName("TradeSessionID");

                entity.Property(e => e.TradeSource)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TsxStampId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TsxStampID");

                entity.Property(e => e.Typ)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.TypeDescription)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CommissiongroupmemberBackup20160717>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("commissiongroupmember_backup_20160717", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.CommissionGroupId).HasColumnName("CommissionGroup_ID");
            });

            modelBuilder.Entity<CommissiongroupmemberBackup20160802>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("commissiongroupmember_backup_20160802", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.CommissionGroupId).HasColumnName("CommissionGroup_ID");
            });

            modelBuilder.Entity<CommissiongroupmemberBackup20170621>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("commissiongroupmember_backup20170621", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.CommissionGroupId).HasColumnName("CommissionGroup_ID");
            });

            modelBuilder.Entity<CompanyProfile>(entity =>
            {
                entity.HasKey(e => e.Usymbol);

                entity.ToTable("CompanyProfile", "dbo");

                entity.HasIndex(e => e.ExchangeTicker, "IX_Company_Ticker")
                    .HasFillFactor((byte)80);

                entity.HasIndex(e => new { e.Exchange, e.ExchangeTicker }, "IX_ExchangeTicket")
                    .HasFillFactor((byte)80);

                entity.HasIndex(e => new { e.ExchangeTicker, e.Exchange }, "Index_CompanyProfile_TickerExchange")
                    .HasFillFactor((byte)80);

                entity.Property(e => e.Usymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");

                entity.Property(e => e.BaseSymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Cusip)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CUSIP");

                entity.Property(e => e.Exchange)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ExchangeTicker)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Isin)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ISIN");

                entity.Property(e => e.Multiplier).HasDefaultValueSql("((1))");

                entity.Property(e => e.SecurityType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CompanyProfileCusipMatch>(entity =>
            {
                entity.HasKey(e => e.Cusip);

                entity.ToTable("CompanyProfile_CUSIP_Match", "dbo");

                entity.Property(e => e.Cusip)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cusip");
            });

            modelBuilder.Entity<CompanyProfileOpt>(entity =>
            {
                entity.HasKey(e => e.Usymbol);

                entity.ToTable("CompanyProfileOpt", "dbo");

                entity.Property(e => e.Usymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");

                entity.Property(e => e.OptionType)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.HasOne(d => d.UsymbolNavigation)
                    .WithOne(p => p.CompanyProfileOpt)
                    .HasForeignKey<CompanyProfileOpt>(d => d.Usymbol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyProfileOpt_CompanyProfile");
            });

            modelBuilder.Entity<CompanyprofileBackup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("companyprofile_backup", "dbo");

                entity.Property(e => e.BaseSymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Cusip)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CUSIP");

                entity.Property(e => e.Exchange)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ExchangeTicker)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SecurityType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Usymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");
            });

            modelBuilder.Entity<CompanyprofileBackup0314>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("companyprofile_backup0314", "dbo");

                entity.Property(e => e.BaseSymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Cusip)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CUSIP");

                entity.Property(e => e.Exchange)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ExchangeTicker)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SecurityType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Usymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");
            });

            modelBuilder.Entity<CompanyprofileBackup20121213>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("companyprofile_backup20121213", "dbo");

                entity.Property(e => e.BaseSymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Cusip)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CUSIP");

                entity.Property(e => e.Exchange)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ExchangeTicker)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Isin)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ISIN");

                entity.Property(e => e.SecurityType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Usymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");
            });

            modelBuilder.Entity<CompanyprofileoptBackup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("companyprofileopt_backup", "dbo");

                entity.Property(e => e.OptionType)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Usymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");
            });

            modelBuilder.Entity<CurrencyList>(entity =>
            {
                entity.HasKey(e => e.Currency);

                entity.ToTable("CurrencyList", "dbo");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CurrencySpread>(entity =>
            {
                entity.HasKey(e => new { e.CurrencyPair, e.ValueMin, e.ValueMax });

                entity.ToTable("CurrencySpread", "dbo");

                entity.Property(e => e.CurrencyPair)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CurrencySpreadGroupId).HasColumnName("CurrencySpreadGroup_ID");

                entity.Property(e => e.CurrencySpreadId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CurrencySpread_ID");

                entity.HasOne(d => d.CurrencySpreadGroup)
                    .WithMany(p => p.CurrencySpreads)
                    .HasForeignKey(d => d.CurrencySpreadGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CurrencySpread_CurrencySpreadGroup");
            });

            modelBuilder.Entity<CurrencySpreadGroup>(entity =>
            {
                entity.ToTable("CurrencySpreadGroup", "dbo");

                entity.Property(e => e.CurrencySpreadGroupId).HasColumnName("CurrencySpreadGroup_ID");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CurrencySpreadGroupMember>(entity =>
            {
                entity.HasKey(e => new { e.CurrencySpreadGroupId, e.AccountId });

                entity.ToTable("CurrencySpreadGroupMember", "dbo");

                entity.Property(e => e.CurrencySpreadGroupId).HasColumnName("CurrencySpreadGroup_ID");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.CurrencySpreadGroupMembers)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CurrencySpreadGroupMember_Account");

                entity.HasOne(d => d.CurrencySpreadGroup)
                    .WithMany(p => p.CurrencySpreadGroupMembers)
                    .HasForeignKey(d => d.CurrencySpreadGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CurrencySpreadGroupMember_CurrencySpreadGroup");
            });

            modelBuilder.Entity<CurrencySpreadView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CurrencySpread_view", "dbo");

                entity.Property(e => e.CurrencyPair)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CurrencySpreadGroupId).HasColumnName("CurrencySpreadGroup_ID");

                entity.Property(e => e.CurrencySpreadId).HasColumnName("CurrencySpread_ID");

                entity.Property(e => e.Groupname)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("groupname");

                entity.Property(e => e.Shortname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("shortname");
            });

            modelBuilder.Entity<CurrentPosition>(entity =>
            {
                entity.HasKey(e => new { e.Usymbol, e.BookId });

                entity.ToTable("CurrentPosition", "dbo");

                entity.HasIndex(e => e.BookId, "IX_CurrentPosition");

                entity.Property(e => e.Usymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");

                entity.Property(e => e.BookId).HasColumnName("Book_ID");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.CurrentPositions)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CurrentPosition_Book");
            });

            modelBuilder.Entity<CurrentpositionBackup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("currentposition_backup", "dbo");

                entity.Property(e => e.BookId).HasColumnName("Book_ID");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.Usymbol)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");
            });

            modelBuilder.Entity<CusipFix>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CUSIP_fix", "dbo");

                entity.Property(e => e.Cusip)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CUSIP");

                entity.Property(e => e.Exchange)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ExchangeTicker)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Das1>(entity =>
            {
                entity.ToTable("DAS1", "dbo");

                entity.HasIndex(e => e.DateInt, "DAS1_Index_Date");

                entity.HasIndex(e => new { e.ProcessDate, e.AccountNumber }, "Index_DateAccount");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountClass)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AccountType)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ClientID");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.CurrentSdbalance).HasColumnName("CurrentSDBalance");

                entity.Property(e => e.CurrentTdbalance).HasColumnName("CurrentTDBalance");

                entity.Property(e => e.DebitCode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.DivConfirm)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.GuranteeAccount)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsertTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Interestcode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.NonRes)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.OptionCode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ResidencyCode)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Rr)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("RR");
            });

            modelBuilder.Entity<Das1AdpView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("das1_adp_view", "dbo");

                entity.Property(e => e.AccountClass)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AccountType)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.AdpaccountCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ADPAccountCode");

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ClientID");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.CurrentSdbalance).HasColumnName("CurrentSDBalance");

                entity.Property(e => e.CurrentTdbalance).HasColumnName("CurrentTDBalance");

                entity.Property(e => e.DebitCode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.DivConfirm)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.GuranteeAccount)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InsertTime).HasColumnType("datetime");

                entity.Property(e => e.Interestcode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.NonRes)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.OptionCode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ResidencyCode)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Rr)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("RR");
            });

            modelBuilder.Entity<Das4>(entity =>
            {
                entity.ToTable("DAS4", "dbo");

                entity.HasIndex(e => new { e.ProcessDate, e.AccountNumber }, "Index_DateAccount");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountCurrency)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsertTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Ismcode)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("ISMCode");

                entity.Property(e => e.SecurityCurrency)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.SecurityDesc)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.SecurityGroupCode)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DebenturePayoutInfo>(entity =>
            {
                entity.HasKey(e => new { e.CouponDateId, e.Usymbol })
                    .IsClustered(false);

                entity.ToTable("DebenturePayoutInfo", "dbo");

                entity.HasIndex(e => new { e.Usymbol, e.CouponDate }, "U_USymbol_CouponDate")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.CouponDateId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CouponDate_ID");

                entity.Property(e => e.Usymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");

                entity.HasOne(d => d.UsymbolNavigation)
                    .WithMany(p => p.DebenturePayoutInfos)
                    .HasForeignKey(d => d.Usymbol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DebenturePayoutInfo_DebentureProfile");
            });

            modelBuilder.Entity<DebentureProfile>(entity =>
            {
                entity.HasKey(e => e.Usymbol)
                    .IsClustered(false);

                entity.ToTable("DebentureProfile", "dbo");

                entity.HasIndex(e => e.Usymbol, "U_USymbol")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.Usymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");

                entity.HasOne(d => d.UsymbolNavigation)
                    .WithOne(p => p.DebentureProfile)
                    .HasForeignKey<DebentureProfile>(d => d.Usymbol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DebentureProfile_CompanyProfile");
            });

            modelBuilder.Entity<Eodreport>(entity =>
            {
                entity.HasKey(e => e.FileDefId);

                entity.ToTable("EODReports", "dbo");

                entity.Property(e => e.FileDefId)
                    .ValueGeneratedNever()
                    .HasColumnName("FileDefID");

                entity.Property(e => e.FileDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EodreportHistory>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.ToTable("EODReportHistory", "dbo");

                entity.Property(e => e.FileId)
                    .ValueGeneratedNever()
                    .HasColumnName("FileID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.FileDefId).HasColumnName("FileDefID");

                entity.HasOne(d => d.FileDef)
                    .WithMany(p => p.EodreportHistories)
                    .HasForeignKey(d => d.FileDefId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EODReports_EODReportHistory");
            });

            modelBuilder.Entity<EquityReportView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("EquityReport_view", "dbo");

                entity.Property(e => e.AmountWcommission).HasColumnName("AmountWCommission");

                entity.Property(e => e.BookId).HasColumnName("Book_ID");

                entity.Property(e => e.BookName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.SettlmntDate).HasColumnType("datetime");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TradeDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Etf>(entity =>
            {
                entity.HasKey(e => new { e.ImportDate, e.Symbol, e.Country });

                entity.ToTable("ETF", "dbo");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(3)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EventHistoryTracker>(entity =>
            {
                entity.ToTable("EventHistoryTracker", "dbo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AcctEventId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("AcctEventID");

                entity.Property(e => e.DtStamp)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_stamp");

                entity.Property(e => e.EventTypeId).HasColumnName("EventTypeID");

                entity.Property(e => e.Updatedfield)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("updatedfield");

                entity.Property(e => e.UserId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("userID");
            });

            modelBuilder.Entity<ExchangeHoliday>(entity =>
            {
                entity.HasKey(e => new { e.Location, e.HolidayDate });

                entity.ToTable("ExchangeHoliday", "dbo");

                entity.Property(e => e.Location)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.HolidayDate).HasColumnType("datetime");

                entity.Property(e => e.ClosingTime).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ExchangeList>(entity =>
            {
                entity.HasKey(e => e.Exchange);

                entity.ToTable("ExchangeList", "dbo");

                entity.Property(e => e.Exchange)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ExchangeRoutingFee>(entity =>
            {
                entity.HasKey(e => new { e.Exchange, e.DestBroker, e.RoutingCode });

                entity.ToTable("ExchangeRoutingFee", "dbo");

                entity.Property(e => e.Exchange)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.DestBroker)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoutingCode)
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Fxrate>(entity =>
            {
                entity.ToTable("FXRate", "dbo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FxrateHistory>(entity =>
            {
                entity.HasKey(e => new { e.Currency, e.DateInt, e.UpdateTime });

                entity.ToTable("FXRateHistory", "dbo");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<GroupMember>(entity =>
            {
                entity.HasKey(e => new { e.BookId, e.GroupId })
                    .HasName("PK_GroupMembers");

                entity.ToTable("GroupMember", "dbo");

                entity.Property(e => e.BookId).HasColumnName("Book_ID");

                entity.Property(e => e.GroupId).HasColumnName("Group_ID");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.GroupMembers)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupMember_Book");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupMembers)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupMembers_BookGroup");
            });

            modelBuilder.Entity<GroupUser>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.GroupId });

                entity.ToTable("GroupUsers", "dbo");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupUserGroups)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupUsers_GroupID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GroupUserUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupUsers_UserID");
            });

            modelBuilder.Entity<ImportLog>(entity =>
            {
                entity.HasKey(e => new { e.ImportType, e.ImportTime });

                entity.ToTable("ImportLog", "dbo");

                entity.Property(e => e.ImportType)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.ImportTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EndId).HasColumnName("End_ID");

                entity.Property(e => e.StartId).HasColumnName("Start_ID");

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UserID");
            });

            modelBuilder.Entity<InsiderSecurity>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.Usymbol });

                entity.ToTable("InsiderSecurity", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.Usymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");

                entity.Property(e => e.RegulationId)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("RegulationID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.InsiderSecurities)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InsiderSecurity_Account");

                entity.HasOne(d => d.UsymbolNavigation)
                    .WithMany(p => p.InsiderSecurities)
                    .HasForeignKey(d => d.Usymbol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InsiderSecurity_CompanyProfile");
            });

            modelBuilder.Entity<InterestRate>(entity =>
            {
                entity.ToTable("InterestRate", "dbo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InterestRateReference>(entity =>
            {
                entity.HasKey(e => new { e.InterestRateCode, e.Currency, e.DateInt })
                    .HasName("PK_InterestRateReference_1");

                entity.ToTable("InterestRateReference", "dbo");

                entity.Property(e => e.InterestRateCode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<InterestSpread>(entity =>
            {
                entity.HasKey(e => new { e.InterestCode, e.UnderlyingInterestCode, e.Currency, e.RangeEnd });

                entity.ToTable("InterestSpread", "dbo");

                entity.Property(e => e.InterestCode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UnderlyingInterestCode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<IntroducingParty>(entity =>
            {
                entity.HasKey(e => new { e.IntroducingParty1, e.AccountCode });

                entity.ToTable("IntroducingParty", "dbo");

                entity.Property(e => e.IntroducingParty1)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("IntroducingParty");

                entity.Property(e => e.AccountCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InvTransDescription>(entity =>
            {
                entity.ToTable("InvTransDescription", "dbo");

                entity.HasIndex(e => e.Description, "IX_Desc")
                    .IsUnique();

                entity.Property(e => e.InvTransDescriptionId).HasColumnName("InvTransDescription_ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InvTransOptionalDatum>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.ToTable("InvTransOptionalData", "dbo");

                entity.Property(e => e.TransactionId)
                    .ValueGeneratedNever()
                    .HasColumnName("Transaction_ID");

                entity.Property(e => e.AllocationId).HasColumnName("Allocation_ID");

                entity.Property(e => e.RelatedUsymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RelatedUSymbol");

                entity.Property(e => e.TransDesc)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Uploaded).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Transaction)
                    .WithOne(p => p.InvTransOptionalDatum)
                    .HasForeignKey<InvTransOptionalDatum>(d => d.TransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvTransOptionalData_InvTransaction");
            });

            modelBuilder.Entity<InvTransPerBookTotalView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("InvTransPerBookTotal_view", "dbo");

                entity.Property(e => e.BookId).HasColumnName("Book_ID");
            });

            modelBuilder.Entity<InvTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId)
                    .HasName("PK_Transactions");

                entity.ToTable("InvTransaction", "dbo");

                entity.HasIndex(e => new { e.BookId, e.Usymbol, e.TradeDateInt }, "IX_InvTransaction_BookID_USymbol")
                    .HasFillFactor((byte)80);

                entity.HasIndex(e => new { e.TradeDateInt, e.Usymbol }, "IX_Transactions");

                entity.Property(e => e.TransactionId).HasColumnName("Transaction_ID");

                entity.Property(e => e.BookId).HasColumnName("Book_ID");

                entity.Property(e => e.BrokerAccountId).HasColumnName("BrokerAccount_ID");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.CxlOrCorrect)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CxlOrCorrectDate).HasColumnType("datetime");

                entity.Property(e => e.Market)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RefTransactionId).HasColumnName("RefTransaction_ID");

                entity.Property(e => e.Side)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Spot).HasDefaultValueSql("((1))");

                entity.Property(e => e.TradeDate).HasColumnType("datetime");

                entity.Property(e => e.TransType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('I')")
                    .IsFixedLength(true);

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserIntId).HasColumnName("User_IntID");

                entity.Property(e => e.Usymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.InvTransactions)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvTransaction_Book");

                entity.HasOne(d => d.BrokerAccount)
                    .WithMany(p => p.InvTransactions)
                    .HasForeignKey(d => d.BrokerAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_BrokerAccount");
            });

            modelBuilder.Entity<InvTransactionConsolidatedView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("InvTransactionConsolidated_view", "dbo");

                entity.Property(e => e.BaseSymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InvTransactionView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("InvTransaction_view", "dbo");

                entity.Property(e => e.AllocationId).HasColumnName("Allocation_ID");

                entity.Property(e => e.BookId).HasColumnName("Book_ID");

                entity.Property(e => e.BrokerAccountId).HasColumnName("BrokerAccount_ID");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.CxlOrCorrect)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CxlOrCorrectDate).HasColumnType("datetime");

                entity.Property(e => e.Market)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RefTransactionId).HasColumnName("RefTransaction_ID");

                entity.Property(e => e.RelatedUsymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RelatedUSymbol");

                entity.Property(e => e.Side)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TradeDate).HasColumnType("datetime");

                entity.Property(e => e.TransDesc)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.TransType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TransactionId).HasColumnName("Transaction_ID");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UserID");

                entity.Property(e => e.UserIntId).HasColumnName("User_IntID");

                entity.Property(e => e.Usymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");
            });

            modelBuilder.Entity<InventoryManagerStatus>(entity =>
            {
                entity.HasKey(e => new { e.BlotterImportTime, e.FxtransImportTime });

                entity.ToTable("InventoryManagerStatus", "dbo");

                entity.Property(e => e.BlotterImportTime).HasColumnType("datetime");

                entity.Property(e => e.FxtransImportTime)
                    .HasColumnType("datetime")
                    .HasColumnName("FXTransImportTime");
            });

            modelBuilder.Entity<LockInfo>(entity =>
            {
                entity.HasKey(e => e.LockId);

                entity.ToTable("LockInfo", "dbo");

                entity.Property(e => e.LockId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LockID");

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.Locked).HasDefaultValueSql("((0))");

                entity.Property(e => e.UserId)
                    .IsUnicode(false)
                    .HasColumnName("UserID")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<LockedRecord>(entity =>
            {
                entity.HasKey(e => e.LockedEndDate);

                entity.ToTable("LockedRecords", "dbo");

                entity.Property(e => e.LockedEndDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<MemberAccount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("member_account", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.MemberId).HasColumnName("member_id");
            });

            modelBuilder.Entity<MemberAccountDoNotUse>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("member_account_do_not_use", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.MemberId).HasColumnName("member_id");
            });

            modelBuilder.Entity<MemberAccountHist>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("member_account_hist", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.ChangeDt)
                    .HasColumnType("datetime")
                    .HasColumnName("change_dt");

                entity.Property(e => e.ChangeTypeCd)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("change_type_cd")
                    .IsFixedLength(true);

                entity.Property(e => e.ChangeUserNm)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("change_user_nm");

                entity.Property(e => e.MemberId).HasColumnName("member_id");
            });

            modelBuilder.Entity<NewAccountDefault>(entity =>
            {
                entity.HasKey(e => e.AccountType);

                entity.ToTable("NewAccountDefaults", "dbo");

                entity.Property(e => e.AccountType)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.NewAccountDefaults)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NewAccountDefaults_Account");
            });

            modelBuilder.Entity<NewAccountExclusion>(entity =>
            {
                entity.HasKey(e => new { e.BranchCd, e.AccountCd, e.TypeAccountCd });

                entity.ToTable("NewAccountExclusion", "dbo");

                entity.Property(e => e.BranchCd)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("branch_cd")
                    .IsFixedLength(true);

                entity.Property(e => e.AccountCd)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("account_cd")
                    .IsFixedLength(true);

                entity.Property(e => e.TypeAccountCd)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("type_account_cd")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<NewIssue>(entity =>
            {
                entity.ToTable("NewIssues", "dbo");

                entity.Property(e => e.NewIssueId).HasColumnName("NewIssue_ID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CounterPartyId).HasColumnName("CounterParty_ID");

                entity.Property(e => e.CreateDateInt).HasDefaultValueSql("((20100608))");

                entity.Property(e => e.HouseId).HasColumnName("House_ID");

                entity.Property(e => e.LastUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('20100608')");

                entity.Property(e => e.UploadInitialAlloc)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Usymbol)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");

                entity.HasOne(d => d.CounterParty)
                    .WithMany(p => p.NewIssueCounterParties)
                    .HasForeignKey(d => d.CounterPartyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CounterParty_Book");

                entity.HasOne(d => d.House)
                    .WithMany(p => p.NewIssueHouses)
                    .HasForeignKey(d => d.HouseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_House_Book");
            });

            modelBuilder.Entity<NewIssueAllocation>(entity =>
            {
                entity.HasKey(e => e.AllocationId)
                    .HasName("PK_NewIssue_Allocations");

                entity.ToTable("NewIssueAllocations", "dbo");

                entity.Property(e => e.AllocationId).HasColumnName("Allocation_ID");

                entity.Property(e => e.BookId).HasColumnName("Book_ID");

                entity.Property(e => e.CxlOrCorrect)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CxlOrCorrectDate).HasColumnType("datetime");

                entity.Property(e => e.NewIssueId).HasColumnName("NewIssue_ID");

                entity.Property(e => e.RefTransactionId).HasColumnName("RefTransaction_ID");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserIntId).HasColumnName("User_IntID");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.NewIssueAllocations)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NewIssueAllocation_Book");

                entity.HasOne(d => d.NewIssue)
                    .WithMany(p => p.NewIssueAllocations)
                    .HasForeignKey(d => d.NewIssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NewIssueAllocation_ID");

                entity.HasOne(d => d.UserInt)
                    .WithMany(p => p.NewIssueAllocations)
                    .HasForeignKey(d => d.UserIntId)
                    .HasConstraintName("FK_NewIssueAllocation_Users");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permissions", "dbo");

                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");

                entity.Property(e => e.AppId).HasColumnName("AppID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.HasOne(d => d.App)
                    .WithMany(p => p.Permissions)
                    .HasForeignKey(d => d.AppId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Permission_Application");
            });

            modelBuilder.Entity<PermissionAssignment>(entity =>
            {
                entity.HasKey(e => e.TblId)
                    .HasName("pk_tbl_id");

                entity.ToTable("PermissionAssignments", "dbo");

                entity.Property(e => e.TblId).HasColumnName("tbl_id");

                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<PermissionAssignmentsBob>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PermissionAssignments_bob", "dbo");

                entity.HasIndex(e => e.UserId, "FK_Clustered_PermissionID")
                    .IsClustered();

                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<PositionGroupReportView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("PositionGroupReport_view", "dbo");

                entity.Property(e => e.BookId).HasColumnName("Book_ID");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Exchange)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ExchangeTicker)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LongShort)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Usymbol)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");
            });

            modelBuilder.Entity<PositionJournalCompare>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Position_Journal_Compare", "dbo");

                entity.Property(e => e.Usymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("usymbol");
            });

            modelBuilder.Entity<PositionReportView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("PositionReport_view", "dbo");

                entity.Property(e => e.BookId).HasColumnName("Book_ID");

                entity.Property(e => e.BookName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Exchange)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ExchangeTicker)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LongShort)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.SecurityType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.Usymbol)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");
            });

            modelBuilder.Entity<PositionWithJournal>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Position_WithJournal", "dbo");

                entity.Property(e => e.Usymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");
            });

            modelBuilder.Entity<PositionWithoutJournal>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Position_WithoutJournal", "dbo");

                entity.Property(e => e.Usymbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");
            });

            modelBuilder.Entity<PromotionalCommissionGroupMember>(entity =>
            {
                entity.HasKey(e => new { e.CommissionGroupId, e.AccountId });

                entity.ToTable("PromotionalCommissionGroupMember", "dbo");

                entity.Property(e => e.CommissionGroupId).HasColumnName("CommissionGroup_ID");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.PromotionalCommissionGroupMembers)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionalCommissionGroupMember_Account");

                entity.HasOne(d => d.CommissionGroup)
                    .WithMany(p => p.PromotionalCommissionGroupMembers)
                    .HasForeignKey(d => d.CommissionGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionalCommissionGroupMember_CommissionGroup");
            });

            modelBuilder.Entity<PromotionalCommissionView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("PromotionalCommission_View", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountTypes)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ActId).HasColumnName("ActID");

                entity.Property(e => e.AddRegulatoryExchangeFeeBeforeOrAfterMinMax)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BmoaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BMOAccountID");

                entity.Property(e => e.Bpsaccount)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("BPSAccount");

                entity.Property(e => e.CommissionId).HasColumnName("CommissionID");

                entity.Property(e => e.DefaultAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultBook)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Exchange)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideAccountType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.PensonAccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonAccountID");

                entity.Property(e => e.PensonUsaccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PensonUSAccountID");

                entity.Property(e => e.SecurityType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TradeSessionId).HasColumnName("TradeSessionID");

                entity.Property(e => e.TradeSource)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TsxStampId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TsxStampID");

                entity.Property(e => e.Typ)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.TypeDescription)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<QuoteHistory>(entity =>
            {
                entity.HasKey(e => new { e.Symbol, e.DateInt, e.ExchangeCode });

                entity.ToTable("QuoteHistory", "dbo");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.ExchangeCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Change).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.Close).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.High).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.Low).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.Open).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.Trades).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Vol).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<QuoteInfo>(entity =>
            {
                entity.HasKey(e => e.Symbolstring);

                entity.ToTable("QuoteInfo", "dbo");

                entity.Property(e => e.Symbolstring)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cusip)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CUSIP");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.DivRate)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ExDivDate)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Exchange)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IssueType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ListingType).IsUnicode(false);
            });

            modelBuilder.Entity<QuoteInfoAllowableSmbListException>(entity =>
            {
                entity.HasKey(e => e.Symbolstring);

                entity.ToTable("QuoteInfoAllowableSmbListExceptions", "dbo");

                entity.Property(e => e.Symbolstring)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AddOrDelete)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Exchange)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<QuoteInfoAllowedSymbol>(entity =>
            {
                entity.HasKey(e => new { e.Symbolstring, e.Exchange });

                entity.ToTable("QuoteInfoAllowedSymbols", "dbo");

                entity.Property(e => e.Symbolstring)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Exchange)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Cusip)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CUSIP");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.DivRate)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ExDivDate)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IssueType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ListingType).IsUnicode(false);
            });

            modelBuilder.Entity<QuoteInfoAllowedSymbolsHistory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("QuoteInfoAllowedSymbolsHistory", "dbo");

                entity.HasIndex(e => new { e.FileDate, e.Exchange }, "Index_QuoteInfoAllowedSymbolsHistory_DateExchange");

                entity.Property(e => e.Cusip)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CUSIP");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.DivRate)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ExDivDate)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Exchange)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FileDate)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IssueType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ListingType).IsUnicode(false);

                entity.Property(e => e.Symbolstring)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RegisteredAccountFee>(entity =>
            {
                entity.HasKey(e => new { e.AccountType, e.Currency })
                    .HasName("PK_RegisteredAccountFees_1");

                entity.ToTable("RegisteredAccountFees", "dbo");

                entity.Property(e => e.AccountType)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.LastDayThreshold)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Prorated)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<RegisteredAccountFxView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("RegisteredAccountFX_view", "dbo");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("currency");

                entity.Property(e => e.Fxrate).HasColumnName("fxrate");

                entity.Property(e => e.Pensonaccountid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("pensonaccountid");

                entity.Property(e => e.Tradedateint).HasColumnName("tradedateint");
            });

            modelBuilder.Entity<RegisteredAccountFxposition>(entity =>
            {
                entity.HasKey(e => new { e.TradeDateInt, e.PensonAccountId })
                    .HasName("PK_RegisteredAccountFX");

                entity.ToTable("RegisteredAccountFXPosition", "dbo");

                entity.Property(e => e.PensonAccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Usdamount).HasColumnName("USDAmount");
            });

            modelBuilder.Entity<RegisteredAccountFxrate>(entity =>
            {
                entity.HasKey(e => e.TradeDateInt);

                entity.ToTable("RegisteredAccountFXRate", "dbo");

                entity.Property(e => e.TradeDateInt).ValueGeneratedNever();

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ResetCommissionGroupLog>(entity =>
            {
                entity.ToTable("ResetCommissionGroup_Log", "dbo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.CommissionGroupId).HasColumnName("CommissionGroup_ID");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<RoutingInstruction>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.Exchange });

                entity.ToTable("RoutingInstruction", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.Exchange)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('TO')");

                entity.Property(e => e.DefaultInstruction)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('S')");

                entity.Property(e => e.OverrideInstruction)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.RoutingInstructionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("RoutingInstructionID");
            });

            modelBuilder.Entity<Securable>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.KeyId, e.GroupId });

                entity.ToTable("Securables", "dbo");

                entity.Property(e => e.ObjectId).HasColumnName("ObjectID");

                entity.Property(e => e.KeyId).HasColumnName("KeyID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");
            });

            modelBuilder.Entity<SecurityGroup>(entity =>
            {
                entity.HasKey(e => e.SecGroupId);

                entity.ToTable("SecurityGroups", "dbo");

                entity.Property(e => e.SecGroupId).HasColumnName("SecGroupID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SecurityType>(entity =>
            {
                entity.ToTable("SecurityTypes", "dbo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AbbrName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("abbr_name");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SettlementInstruction>(entity =>
            {
                entity.HasKey(e => e.InstructionId);

                entity.ToTable("SettlementInstruction", "dbo");

                entity.Property(e => e.InstructionId).HasColumnName("Instruction_ID");

                entity.Property(e => e.FutSettDate).HasColumnType("datetime");

                entity.Property(e => e.SettlmntTyp)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TradeDateEnd).HasColumnType("datetime");

                entity.Property(e => e.TradeDateStart).HasColumnType("datetime");

                entity.Property(e => e.Usymbol)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USymbol");
            });

            modelBuilder.Entity<Sl>(entity =>
            {
                entity.ToTable("SL", "dbo");

                entity.HasIndex(e => new { e.ProcessDate, e.AccountNumber }, "Index_DateAccount");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountCredited)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_NUMBER");

                entity.Property(e => e.BbscalcPrice).HasColumnName("BBSCalcPrice");

                entity.Property(e => e.BrAcctType)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("BR_ACCT_TYPE");

                entity.Property(e => e.ChargeTotalTran).HasColumnName("Charge_Total_Tran");

                entity.Property(e => e.CusipNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CUSIP_NO");

                entity.Property(e => e.DayYear).HasColumnName("Day_Year");

                entity.Property(e => e.EngShortDesc)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ENG_SHORT_DESC");

                entity.Property(e => e.IbmSecurityId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IBM_SECURITY_ID");

                entity.Property(e => e.InsertTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MarketPrice).HasColumnName("MARKET_PRICE");

                entity.Property(e => e.Minloanet).HasColumnName("MINLOANET");

                entity.Property(e => e.NoDays).HasColumnName("No_Days");

                entity.Property(e => e.OvernightRate).HasColumnName("OVERNIGHT_RATE");

                entity.Property(e => e.ProcessDate).HasColumnName("PROCESS_DATE");

                entity.Property(e => e.SecurityAdpNbr)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("security_adp_nbr")
                    .IsFixedLength(true);

                entity.Property(e => e.SecurityClass)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("SECURITY_CLASS");

                entity.Property(e => e.SecurityFunds)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("SECURITY_FUNDS");

                entity.Property(e => e.SecurityType)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("SECURITY_TYPE");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SYMBOL");
            });

            modelBuilder.Entity<StrategyType>(entity =>
            {
                entity.HasKey(e => e.Type);

                entity.ToTable("StrategyType", "dbo");

                entity.HasIndex(e => e.Description, "IX_StrategyTypeDesc")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TempTimeMarker>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("temp_time_marker", "dbo");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_time");
            });

            modelBuilder.Entity<Tr>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TR", "dbo");

                entity.HasIndex(e => new { e.Td, e.Account }, "Index_DateAccount");

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Comm).HasColumnName("COMM");

                entity.Property(e => e.Cusip)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("CUSIP");

                entity.Property(e => e.Fx).HasColumnName("FX");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.InsertTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Mkt)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MKT");

                entity.Property(e => e.OpenClose)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Qty).HasColumnName("QTY");

                entity.Property(e => e.Sd).HasColumnName("SD");

                entity.Property(e => e.Side)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Tb)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("TB");

                entity.Property(e => e.Td).HasColumnName("TD");
            });

            modelBuilder.Entity<TradeDbBrokerMap>(entity =>
            {
                entity.HasKey(e => e.BrokerName);

                entity.ToTable("TradeDB_BrokerMap", "dbo");

                entity.Property(e => e.BrokerName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BrokerShortNamePrefix)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Broker_ShortName_Prefix");
            });

            modelBuilder.Entity<TradeDbCurrencyMap>(entity =>
            {
                entity.HasKey(e => e.Currency);

                entity.ToTable("TradeDB_CurrencyMap", "dbo");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.CurrencyShortName)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("Currency_ShortName");
            });

            modelBuilder.Entity<TradeDbExchangeMap>(entity =>
            {
                entity.HasKey(e => e.Exchange);

                entity.ToTable("TradeDB_ExchangeMap", "dbo");

                entity.Property(e => e.Exchange)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.InventoryExchange)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TradeDbTraderBookMap>(entity =>
            {
                entity.HasKey(e => e.TraderId);

                entity.ToTable("TradeDB_TraderBookMap", "dbo");

                entity.Property(e => e.TraderId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TraderID");

                entity.Property(e => e.BookShortNamePrefix)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Book_ShortName_Prefix");
            });

            modelBuilder.Entity<Trader>(entity =>
            {
                entity.ToTable("Trader", "dbo");

                entity.Property(e => e.TraderId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Trader_ID");

                entity.Property(e => e.TraderDescription).HasMaxLength(50);

                entity.Property(e => e.TraderType)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TraderAccessGroup>(entity =>
            {
                entity.HasKey(e => new { e.TraderGroupId, e.AccountId });

                entity.ToTable("TraderAccessGroup", "dbo");

                entity.Property(e => e.TraderGroupId).HasColumnName("TraderGroup_ID");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.TraderAccessGroups)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TraderAccessGroup_Account");

                entity.HasOne(d => d.TraderGroup)
                    .WithMany(p => p.TraderAccessGroups)
                    .HasForeignKey(d => d.TraderGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TraderAccessGroup_TraderAccountGroup");
            });

            modelBuilder.Entity<TraderAccount>(entity =>
            {
                entity.HasKey(e => new { e.TraderId, e.AccountId })
                    .HasName("PK_DefaultAccount");

                entity.ToTable("TraderAccount", "dbo");

                entity.Property(e => e.TraderId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Trader_ID");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.TraderAccounts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DefaultAccount_AccountID");
            });

            modelBuilder.Entity<TraderAccountDmagatewayView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("TraderAccount_DMAGateway_view", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Shortname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("shortname");

                entity.Property(e => e.TraderId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("trader_ID");
            });

            modelBuilder.Entity<TraderAccountGroup>(entity =>
            {
                entity.HasKey(e => e.TraderGroupId);

                entity.ToTable("TraderAccountGroup", "dbo");

                entity.Property(e => e.TraderGroupId).HasColumnName("TraderGroup_ID");

                entity.Property(e => e.TraderGroupName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('G')")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<TraderAccountGroupMember>(entity =>
            {
                entity.HasKey(e => new { e.TraderGroupId, e.TraderId });

                entity.ToTable("TraderAccountGroupMembers", "dbo");

                entity.Property(e => e.TraderGroupId).HasColumnName("TraderGroup_ID");

                entity.Property(e => e.TraderId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Trader_ID");

                entity.HasOne(d => d.TraderGroup)
                    .WithMany(p => p.TraderAccountGroupMembers)
                    .HasForeignKey(d => d.TraderGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TraderAccountGroupMembers_TraderAccountGroup");
            });

            modelBuilder.Entity<TraderAccountView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("TraderAccount_view", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.TraderId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("trader_ID");
            });

            modelBuilder.Entity<TraderAccountView2>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("TraderAccount_view2", "dbo");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Shortname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("shortname");

                entity.Property(e => e.TraderId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("trader_ID");
            });

            modelBuilder.Entity<TraderBookGroupAccess>(entity =>
            {
                entity.HasKey(e => new { e.BookGroupId, e.TraderId });

                entity.ToTable("TraderBookGroupAccess", "dbo");

                entity.Property(e => e.BookGroupId).HasColumnName("BookGroup_ID");

                entity.Property(e => e.TraderId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Trader_ID");

                entity.HasOne(d => d.BookGroup)
                    .WithMany(p => p.TraderBookGroupAccesses)
                    .HasForeignKey(d => d.BookGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TraderBookGroupAccess_BookGroup");

                entity.HasOne(d => d.Trader)
                    .WithMany(p => p.TraderBookGroupAccesses)
                    .HasForeignKey(d => d.TraderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TraderBookGroupAccess_Trader");
            });

            modelBuilder.Entity<TransactionColumnOverride>(entity =>
            {
                entity.HasKey(e => new { e.FileDefId, e.FileColumnId });

                entity.ToTable("TransactionColumnOverride", "dbo");

                entity.Property(e => e.FileDefId).HasColumnName("FileDefID");

                entity.Property(e => e.FileColumnId).HasColumnName("FileColumnID");

                entity.HasOne(d => d.FileColumn)
                    .WithMany(p => p.TransactionColumnOverrides)
                    .HasForeignKey(d => d.FileColumnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionColumnOverride_TransactionFileColumn");

                entity.HasOne(d => d.FileDef)
                    .WithMany(p => p.TransactionColumnOverrides)
                    .HasForeignKey(d => d.FileDefId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionColumnOverride_TransactionColumnOverride");
            });

            modelBuilder.Entity<TransactionFile>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.ToTable("TransactionFile", "dbo");

                entity.Property(e => e.FileId).HasColumnName("FileID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatorId).HasColumnName("CreatorID");

                entity.Property(e => e.FileDefId).HasColumnName("FileDefID");

                entity.Property(e => e.ManagerApprovalTime).HasColumnType("datetime");

                entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SupervisorApprovalTime).HasColumnType("datetime");

                entity.Property(e => e.SupervisorId).HasColumnName("SupervisorID");

                entity.Property(e => e.UploadId).HasColumnName("UploadID");

                entity.Property(e => e.UploadTime).HasColumnType("datetime");

                entity.HasOne(d => d.FileDef)
                    .WithMany(p => p.TransactionFiles)
                    .HasForeignKey(d => d.FileDefId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionFile_TransactionFileDefinition");
            });

            modelBuilder.Entity<TransactionFileColumn>(entity =>
            {
                entity.HasKey(e => e.FileColumnId);

                entity.ToTable("TransactionFileColumn", "dbo");

                entity.Property(e => e.FileColumnId).HasColumnName("FileColumnID");

                entity.Property(e => e.FileTypeId)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("FileTypeID");

                entity.Property(e => e.IncludeInUpload)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UseInTotals)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.FileType)
                    .WithMany(p => p.TransactionFileColumns)
                    .HasForeignKey(d => d.FileTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionFileColumn_TransactionFileType");
            });

            modelBuilder.Entity<TransactionFileColumnDefault>(entity =>
            {
                entity.HasKey(e => new { e.FileColumnId, e.FileDefId });

                entity.ToTable("TransactionFileColumnDefault", "dbo");

                entity.Property(e => e.FileColumnId).HasColumnName("FileColumnID");

                entity.Property(e => e.FileDefId).HasColumnName("FileDefID");

                entity.Property(e => e.FieldDefault)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasOne(d => d.FileColumn)
                    .WithMany(p => p.TransactionFileColumnDefaults)
                    .HasForeignKey(d => d.FileColumnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionFileColumnDefault_TransactionFileColumn");

                entity.HasOne(d => d.FileDef)
                    .WithMany(p => p.TransactionFileColumnDefaults)
                    .HasForeignKey(d => d.FileDefId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionFileColumnDefault_TransactionFileDefinition");
            });

            modelBuilder.Entity<TransactionFileDefinition>(entity =>
            {
                entity.HasKey(e => e.FileDefId);

                entity.ToTable("TransactionFileDefinition", "dbo");

                entity.Property(e => e.FileDefId).HasColumnName("FileDefID");

                entity.Property(e => e.CreateGroupId).HasColumnName("CreateGroupID");

                entity.Property(e => e.FileDescription)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.FileTypeId)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("FileTypeID");

                entity.Property(e => e.ManagerGroupId).HasColumnName("ManagerGroupID");

                entity.Property(e => e.SupervisorGroupId).HasColumnName("SupervisorGroupID");

                entity.HasOne(d => d.FileType)
                    .WithMany(p => p.TransactionFileDefinitions)
                    .HasForeignKey(d => d.FileTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionFileDefinition_TransactionFileType");
            });

            modelBuilder.Entity<TransactionFileRecord>(entity =>
            {
                entity.HasKey(e => e.FileRecordId);

                entity.ToTable("TransactionFileRecord", "dbo");

                entity.Property(e => e.FileRecordId).HasColumnName("FileRecordID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Excluded)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Note)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.RecordData)
                    .IsRequired()
                    .HasColumnType("xml");

                entity.Property(e => e.RecordTypeId).HasColumnName("RecordTypeID");

                entity.Property(e => e.TransactionFileId).HasColumnName("TransactionFileID");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.HasOne(d => d.RecordType)
                    .WithMany(p => p.TransactionFileRecords)
                    .HasForeignKey(d => d.RecordTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionFileRecord_TransactionFileRecord");

                entity.HasOne(d => d.TransactionFile)
                    .WithMany(p => p.TransactionFileRecords)
                    .HasForeignKey(d => d.TransactionFileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionFileRecord_TransactionFile");
            });

            modelBuilder.Entity<TransactionFileRecordType>(entity =>
            {
                entity.HasKey(e => e.RecordTypeId);

                entity.ToTable("TransactionFileRecordType", "dbo");

                entity.Property(e => e.RecordTypeId).HasColumnName("RecordTypeID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TransactionFileType>(entity =>
            {
                entity.HasKey(e => e.FileTypeId);

                entity.ToTable("TransactionFileType", "dbo");

                entity.Property(e => e.FileTypeId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("FileTypeID");

                entity.Property(e => e.FileTypeDescription)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserIntId)
                    .HasName("PK_User");

                entity.ToTable("Users", "dbo");

                entity.Property(e => e.UserIntId).HasColumnName("User_IntID");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UserID");
            });

            modelBuilder.Entity<UserAccessAccount>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("UserAccessAccount", "dbo");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.LoginAcct)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("login_acct");

                entity.Property(e => e.PermissionRights)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('w')");
            });

            modelBuilder.Entity<UserSecGroupLevel>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("UserSecGroupLevel", "dbo");

                entity.Property(e => e.Secgroupid).HasColumnName("secgroupid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSecGroupLevel_UserAccessAccount");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
