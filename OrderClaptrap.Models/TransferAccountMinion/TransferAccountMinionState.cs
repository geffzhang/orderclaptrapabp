using Newbe.Claptrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClaptrap.Models.TransferAccountMinion
{
    public class TransferAccountMinionState : IStateData
    {
        public Guid AccountId { get; set; }

        public Guid RemoteAccountId { get; set; }

        public decimal BeforBalance { get; set; }
        public decimal AfterBalance { get; set; }

        public DateTime BalanceTime { get; set; }

        public decimal Amount { get; set; }
    }
}