using Dapr.Actors.Runtime;
using Newbe.Claptrap;
using Newbe.Claptrap.Dapr;
using OrderClaptrap.IActor;
using OrderClaptrap.Models;
using OrderClaptrap.Models.Accounts;

namespace OrderClaptrap.Actors.TransferAccountMinion
{
    [Actor(TypeName = ClaptrapCodes.TransferAccountMinionActor)]
    [ClaptrapEventHandler(typeof(Events.TransferAccountEventHanlder), ClaptrapCodes.TransferAccountEvent)]
    [ClaptrapEventHandler(typeof(Events.TransferAccountCompletedEventHandler), ClaptrapCodes.TransferAccountCompletedEvent)]
    public class TransferAccountMinionActor : ClaptrapBoxActor<NoneStateData>,
        ITransferAccountMinionActor
    {
        public TransferAccountMinionActor(ActorHost actorHost,
            IClaptrapActorCommonService claptrapActorCommonService)
            : base(actorHost, claptrapActorCommonService)
        {
        }
    }
}