using System.Collections.Generic;
using System.Threading.Tasks;
using Dapr.Actors.Runtime;
using OrderClaptrap.Actors.AuctionItemUserCountMinion.Events;
using OrderClaptrap.IActor;
using OrderClaptrap.Models;
using OrderClaptrap.Models.AuctionItemUserCountMinion;
using Newbe.Claptrap;
using Newbe.Claptrap.Dapr;
using System;
using OrderClaptrap.Models.AuctionItem.Events;

namespace OrderClaptrap.Actors.AuctionItemUserCountMinion
{
    [Actor(TypeName = ClaptrapCodes.AuctionItemUserCountMinionActor)]
    [ClaptrapEventHandler(typeof(NewBidderEventHandler), ClaptrapCodes.NewBidderEvent)]
    public class AuctionItemUserCountMinionActor : ClaptrapBoxActor<AuctionItemUserCountState>,
        IAuctionItemUserCountMinionActor
    {
        public AuctionItemUserCountMinionActor(ActorHost actorHost,
            IClaptrapActorCommonService claptrapActorCommonService)
            : base(actorHost, claptrapActorCommonService)
        {
        }

        public Task<Dictionary<Guid, int>> GetUserBiddingCountAsync()
        {
            return Task.FromResult(StateData.UserBiddingCount);
        }
    }
}