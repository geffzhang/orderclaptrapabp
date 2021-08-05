using System.Collections.Generic;
using System.Threading.Tasks;
using OrderClaptrap.Models;
using OrderClaptrap.Models.AuctionItemUserCountMinion;
using Newbe.Claptrap;
using Newbe.Claptrap.Dapr.Core;
using System;
using OrderClaptrap.Models.AuctionItem.Events;

namespace OrderClaptrap.IActor
{
    [ClaptrapMinion(ClaptrapCodes.AuctionItemActor)]
    [ClaptrapState(typeof(AuctionItemUserCountState), ClaptrapCodes.AuctionItemUserCountMinionActor)]
    public interface IAuctionItemUserCountMinionActor : IClaptrapMinionActor
    {
        Task<Dictionary<Guid, int>> GetUserBiddingCountAsync();
    }
}