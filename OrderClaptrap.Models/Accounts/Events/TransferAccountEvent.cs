using Newbe.Claptrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClaptrap.Models.Accounts.Events
{
    public class TransferAccountEvent : IEventData
    {
        public Guid AccountId { get; set; }
        public Guid RemoteAccountId { get; set; }

        public DateTime DateTime { get; set; }

        public decimal Amount { get; set; }
    }
}