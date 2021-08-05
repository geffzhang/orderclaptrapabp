using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderClaptrap.EntityFrameworkCore.Entities
{
    public class Account : FullAuditedAggregateRoot<Guid>, IHasExtraProperties
    {
        public string Name { get; protected set; }

        public decimal Balance { get; protected set; }

        public Account()
        {
        }

        public Account(Guid id,
            string name)
            : base(id)
        {
            Name = name;
            Balance = 600;
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

        public decimal BalanceFromState(decimal balance)
        {
            Balance = balance;
            return Balance;
        }
    }
}