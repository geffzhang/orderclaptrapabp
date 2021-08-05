using Dapr.Actors.Client;
using Newbe.Claptrap;
using Newbe.Claptrap.Dapr;
using OrderClaptrap.EntityFrameworkCore.Entities;
using OrderClaptrap.IActor;
using OrderClaptrap.Models.Accounts.Events;
using OrderClaptrap.Models.TransferAccountMinion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClaptrap.Actors.TransferAccountMinion.Events
{
    public class TransferAccountCompletedEventHandler : NormalEventHandler<NoneStateData, TransferAccountCompletedEvent>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IActorProxyFactory _actorProxyFactory;

        public TransferAccountCompletedEventHandler(IAccountRepository accountRepository,
            IActorProxyFactory actorProxyFactory)
        {
            _accountRepository = accountRepository;
            _actorProxyFactory = actorProxyFactory;
        }

        public override ValueTask HandleEvent(NoneStateData stateData, TransferAccountCompletedEvent eventData, IEventContext eventContext)
        {
            var accountActor = _actorProxyFactory.GetClaptrap<ITransferAccountActor>(eventData.AccountId.ToString());
            var remoteAccountActor = _actorProxyFactory.GetClaptrap<ITransferAccountActor>(eventData.RemoteAccountId.ToString());

            accountActor.TransferCompleted();
            remoteAccountActor.TransferCompleted();

            return ValueTask.CompletedTask;
        }
    }
}