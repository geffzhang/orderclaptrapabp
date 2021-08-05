using Newbe.Claptrap;
using OrderClaptrap.EntityFrameworkCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClaptrap.Models.Accounts
{
    public class TransferAccountState : IStateData
    {
        public Guid AccountId { get; set; }

        public decimal Balance { get; protected set; }

        public SortedDictionary<DateTime, TransferAccountInRecord> TransferAccountInRecords { get; set; }

        public SortedDictionary<DateTime, TransferAccountOutRecord> TransferAccountOutRecords { get; set; }

        public TransferAccountState()
        {
        }

        public TransferAccountState(Guid accountId, decimal balance
            )
        {
            AccountId = accountId;
            Balance = balance;
        }

        public static TransferAccountState Create(Account account)
        {
            return new TransferAccountState(account.Id, account.Balance);
        }

        public void InitTransferAccountOutRecords()
        {
            TransferAccountOutRecords = new SortedDictionary<DateTime, TransferAccountOutRecord>(
                Comparer<DateTime>.Create((x, y) => Comparer<DateTime>.Default.Compare(y, x)));
        }

        public void InitTransferAccountInRecords()
        {
            TransferAccountInRecords = new SortedDictionary<DateTime, TransferAccountInRecord>(
                Comparer<DateTime>.Create((x, y) => Comparer<DateTime>.Default.Compare(y, x)));
        }

        public decimal AddBalance(decimal amount)
        {
            Balance += amount;
            return Balance;
        }

        public decimal SubBalance(decimal amount)
        {
            Balance -= amount;
            return Balance;
        }
    }

    public record TransferAccountInRecord
    {
        public Guid RemoteAccountId { get; set; }

        public DateTime DateTime { get; set; }

        public decimal Amount { get; set; }
    }
    public record TransferAccountOutRecord
    {
        public Guid RemoteAccountId { get; set; }

        public DateTime DateTime { get; set; }

        public decimal Amount { get; set; }
    }
}