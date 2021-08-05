using Dapr.Actors.Runtime;
using Newbe.Claptrap;
using Newbe.Claptrap.Dapr;
using OrderClaptrap.IActor;
using OrderClaptrap.Models;
using System;
using System.Threading.Tasks;

namespace OrderClaptrap.Actors.AuctionItemAccountBalanceMinion
{
    //[Actor(TypeName = ClaptrapCodes.AuctionItemAccountBalanceMinionActor)]
    //[ClaptrapEventHandler(typeof(DeductAccountBalanceEventHandler), ClaptrapCodes.NewBidderEvent)]
    //public class AuctionItemAccountBalanceMinionActor : ClaptrapBoxActor<AuctionItemAccountBalanceState>,
    //    IAuctionItemAccountBalanceMinionActor
    //{
    //    public AuctionItemAccountBalanceMinionActor(ActorHost actorHost,
    //        IClaptrapActorCommonService claptrapActorCommonService)
    //        : base(actorHost, claptrapActorCommonService)
    //    {
    //    }

    //    public Task DeductAccountBalance(NewBidderEvent data)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}