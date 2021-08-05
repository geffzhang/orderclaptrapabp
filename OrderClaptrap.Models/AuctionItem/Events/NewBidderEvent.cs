using Newbe.Claptrap;
using System;

namespace OrderClaptrap.Models.AuctionItem.Events
{
    public record NewBidderEvent : IEventData
    {
        public Guid UserId { get; set; }
        public decimal Price { get; set; }

        public int ItemId { get; set; }
    }
}