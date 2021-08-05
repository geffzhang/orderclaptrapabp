using System.Collections.Generic;
using System.Threading.Tasks;
using OrderClaptrap.Models.AuctionItem.Events;
using OrderClaptrap.Models.AuctionItemUserCountMinion;
using Newbe.Claptrap;
using System;
using Dapr.Actors.Client;
using Newbe.Claptrap.Dapr;
using OrderClaptrap.IActor;

namespace OrderClaptrap.Actors.AuctionItemUserCountMinion.Events
{
    public class NewBidderEventHandler
        : NormalEventHandler<AuctionItemUserCountState, NewBidderEvent>
    {
        private readonly IActorProxyFactory _actorProxyFactory;

        public NewBidderEventHandler(IActorProxyFactory actorProxyFactory)
        {
            _actorProxyFactory = actorProxyFactory;
        }

        public override ValueTask HandleEvent(AuctionItemUserCountState stateData, NewBidderEvent eventData,
            IEventContext eventContext)
        {
            var dic = stateData.UserBiddingCount ?? new Dictionary<Guid, int>();
            var userId = eventData.UserId;
            if (!dic.TryGetValue(userId, out var nowValue))
            {
                nowValue = 0;
            }

            nowValue++;
            dic[userId] = nowValue;
            stateData.UserBiddingCount = dic;
            return ValueTask.CompletedTask;
        }
    }
}