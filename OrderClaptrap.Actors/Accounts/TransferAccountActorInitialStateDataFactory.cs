using Newbe.Claptrap;
using OrderClaptrap.EntityFrameworkCore.Entities;
using OrderClaptrap.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClaptrap.Actors.Accounts
{
    public class TransferAccountActorInitialStateDataFactory : IInitialStateDataFactory
    {
        private readonly IAccountRepository _accountRepository;

        public TransferAccountActorInitialStateDataFactory(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IStateData> Create(IClaptrapIdentity identity)
        {
            Guid.TryParse(identity.Id, out Guid accountId);
            var account = await _accountRepository.GetAsync(accountId);

            return TransferAccountState.Create(account);
        }
    }
}