using Dapr.Actors.Client;
using Newbe.Claptrap;
using Newbe.Claptrap.Dapr;
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
    public class TransferAccountEventHanlder : NormalEventHandler<NoneStateData, TransferAccountEvent>
    {
        private readonly IActorProxyFactory _actorProxyFactory;

        public TransferAccountEventHanlder(IActorProxyFactory actorProxyFactory)
        {
            _actorProxyFactory = actorProxyFactory;
        }

        public override async ValueTask HandleEvent(NoneStateData stateData, TransferAccountEvent eventData, IEventContext eventContext)
        {
            var actor = _actorProxyFactory.GetClaptrap<ITransferAccountActor>(eventData.RemoteAccountId.ToString());

            var input = new TryTransferAccountInput()
            {
                AccountId = eventData.AccountId,
                Amount = eventData.Amount,
                RemoteAccountId = eventData.RemoteAccountId
            };
            var result = await actor.TryTransferInAccount(input);
            if (!result.Success)
            {
                throw new Exception("transferAccount in failed");
            }
        }
    }
}