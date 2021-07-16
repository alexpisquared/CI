using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class Book
    {
        public Book()
        {
            CapTrans = new HashSet<CapTran>();
            CapTransHists = new HashSet<CapTransHist>();
            CashTrans = new HashSet<CashTran>();
            CurrentPositions = new HashSet<CurrentPosition>();
            GroupMembers = new HashSet<GroupMember>();
            InvTransactions = new HashSet<InvTransaction>();
            NewIssueAllocations = new HashSet<NewIssueAllocation>();
            NewIssueCounterParties = new HashSet<NewIssue>();
            NewIssueHouses = new HashSet<NewIssue>();
        }

        public int BookId { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public string ReportingCurrency { get; set; }
        public string AccountNum { get; set; }
        public string DefMarket { get; set; }
        public string ShortName { get; set; }
        public int StrategyType { get; set; }
        public int Active { get; set; }

        public virtual StrategyType StrategyTypeNavigation { get; set; }
        public virtual BookDefaultHouse BookDefaultHouse { get; set; }
        public virtual ICollection<CapTran> CapTrans { get; set; }
        public virtual ICollection<CapTransHist> CapTransHists { get; set; }
        public virtual ICollection<CashTran> CashTrans { get; set; }
        public virtual ICollection<CurrentPosition> CurrentPositions { get; set; }
        public virtual ICollection<GroupMember> GroupMembers { get; set; }
        public virtual ICollection<InvTransaction> InvTransactions { get; set; }
        public virtual ICollection<NewIssueAllocation> NewIssueAllocations { get; set; }
        public virtual ICollection<NewIssue> NewIssueCounterParties { get; set; }
        public virtual ICollection<NewIssue> NewIssueHouses { get; set; }
    }
}
