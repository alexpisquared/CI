using System;
using System.Collections.Generic;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
    public partial class BrokerAccount
    {
        public BrokerAccount()
        {
            BrokerDefaultHouseBrokerAccounts = new HashSet<BrokerDefaultHouse>();
            BrokerDefaultHouseHouses = new HashSet<BrokerDefaultHouse>();
            InvTransactions = new HashSet<InvTransaction>();
        }

        public int BrokerAccountId { get; set; }
        public string AccountName { get; set; }
        public string Currency { get; set; }
        public string BrokerName { get; set; }
        public string ShortAccountName { get; set; }
        public short BrokerType { get; set; }

        public virtual ICollection<BrokerDefaultHouse> BrokerDefaultHouseBrokerAccounts { get; set; }
        public virtual ICollection<BrokerDefaultHouse> BrokerDefaultHouseHouses { get; set; }
        public virtual ICollection<InvTransaction> InvTransactions { get; set; }
    }
}
