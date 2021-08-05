using System.Collections.Generic;
using OrderClaptrap.Models.AuctionItem;
using Newbe.Claptrap;
using System;

namespace OrderClaptrap.Models.AuctionItemUserCountMinion
{
    public class AuctionItemUserCountState : IStateData
    {
        public Dictionary<Guid, int> UserBiddingCount { get; set; } = new();
    }
}