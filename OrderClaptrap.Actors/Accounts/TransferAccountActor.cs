using Dapr.Actors.Runtime;
using Newbe.Claptrap;
using Newbe.Claptrap.Dapr;
using Newbe.Claptrap.StorageProvider.Relational.StateStore;
using OrderClaptrap.Actors.Accounts.Events;
using OrderClaptrap.EntityFrameworkCore.Entities;
using OrderClaptrap.IActor;
using OrderClaptrap.Models;
using OrderClaptrap.Models.Accounts;
using OrderClaptrap.Models.Accounts.Events;
using System;
using System.Threading.Tasks;

namespace OrderClaptrap.Actors.Accounts
{
    [Actor(TypeName = ClaptrapCodes.TransferAccountActor)]
    [ClaptrapStateInitialFactoryHandler(typeof(TransferAccountActorInitialStateDataFactory))]
    //[ClaptrapStateStore(null, typeof(DecoratedStateLoaderFactory<AuctionItemActorStateLoader>))]
    [ClaptrapEventHandler(typeof(TransferAccountEventHanlder), ClaptrapCodes.TransferAccountEvent)]
    [ClaptrapEventHandler(typeof(TransferAccountCompletedEventHandler), ClaptrapCodes.TransferAccountCompletedEvent)]
    public class TransferAccountActor : ClaptrapBoxActor<TransferAccountState>, ITransferAccountActor
    {
        private readonly IAccountRepository _accountRepository;

        public TransferAccountActor(ActorHost actorHost,
            IClaptrapActorCommonService claptrapActorCommonService,
            IAccountRepository accountRepository
            ) : base(actorHost, claptrapActorCommonService)
        {
            _accountRepository = accountRepository;
        }

        private decimal GetCurrentBalance()
        {
            return StateData.Balance;
        }

        public async Task<TryTransferAccountResult> TryTransferOutAccount(TryTransferAccountInput input)
        {
            var remoteAccount = await _accountRepository.FindAsync(a => a.Id == input.RemoteAccountId);
            var account = await _accountRepository.FindAsync(a => a.Id == input.AccountId);
            if (remoteAccount == null || account == null)
            {
                throw new System.Exception("account not found");
            }
            if (account.Balance < input.Amount)
            {
                throw new System.Exception("account balance less than amount");
            }

            var evt = this.CreateEvent(new TransferAccountEvent
            {
                AccountId = input.AccountId,
                RemoteAccountId = input.RemoteAccountId,
                Amount = input.Amount,
                DateTime = DateTime.Now
            });
            await Claptrap.HandleEventAsync(evt);

            return new TryTransferAccountResult()
            {
                Success = true,
                UserId = input.AccountId,
                NowBalance = GetCurrentBalance()
            };
        }

        public async Task<TryTransferAccountResult> TryTransferInAccount(TryTransferAccountInput input)
        {
            var remoteAccount = await _accountRepository.FindAsync(a => a.Id == input.RemoteAccountId);
            if (remoteAccount == null)
            {
                throw new System.Exception("account not found");
            }

            var evt = this.CreateEvent(new TransferAccountCompletedEvent
            {
                AccountId = input.AccountId,
                RemoteAccountId = input.RemoteAccountId,
                Amount = input.Amount,
                DateTime = DateTime.Now
            });
            await Claptrap.HandleEventAsync(evt);

            return new TryTransferAccountResult()
            {
                Success = true
            };
        }

        public async Task<TryTransferAccountResult> TryTransferAccount(TryTransferAccountInput input)
        {
            var remoteAccount = await _accountRepository.FindAsync(a => a.Id == input.RemoteAccountId);
            var account = await _accountRepository.FindAsync(a => a.Id == input.AccountId);
            if (remoteAccount == null || account == null)
            {
                throw new System.Exception("account not found");
            }
            if (account.Balance < input.Amount)
            {
                throw new System.Exception("account balance less than amount");
            }

            var evt = this.CreateEvent(new TransferAccountEvent
            {
                AccountId = input.AccountId,
                RemoteAccountId = input.RemoteAccountId,
                Amount = input.Amount,
                DateTime = DateTime.Now
            });
            await Claptrap.HandleEventAsync(evt);

            account.SubBalance(input.Amount);
            await _accountRepository.UpdateAsync(account);

            return new TryTransferAccountResult()
            {
                Success = true,
                UserId = input.AccountId,
                NowBalance = GetCurrentBalance()
            };
        }

        public async Task TransferCompleted()
        {
            var account = await _accountRepository.GetAsync(StateData.AccountId);

            account.BalanceFromState(StateData.Balance);

            await _accountRepository.UpdateAsync(account);
        }
    }
}