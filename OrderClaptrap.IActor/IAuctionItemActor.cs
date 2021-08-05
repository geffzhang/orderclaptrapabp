using System.Threading.Tasks;
using OrderClaptrap.Models;
using OrderClaptrap.Models.AuctionItem;
using OrderClaptrap.Models.AuctionItem.Events;
using Newbe.Claptrap;
using Newbe.Claptrap.Dapr.Core;
using System;

namespace OrderClaptrap.IActor
{
    [ClaptrapState(typeof(AuctionItemState), ClaptrapCodes.AuctionItemActor)]
    [ClaptrapEvent(typeof(NewBidderEvent), ClaptrapCodes.NewBidderEvent)]
    public interface IAuctionItemActor : IClaptrapActor
    {
        Task<AuctionItemStatus> GetStatusAsync();

        Task<TryBiddingResult> TryBidding(TryBiddingInput input);

        Task<AuctionItemState> GetStateAsync();

        Task<decimal> GetTopPriceAsync();
    }

    public record TryBiddingResult
    {
        public bool Success { get; set; }
        public Guid UserId { get; set; }
        public AuctionItemStatus AuctionItemStatus { get; set; }
        public decimal NowPrice { get; set; }

        public string Message { get; set; }
    }

    public record TryBiddingInput
    {
        public Guid UserId { get; set; }
        public decimal Price { get; set; }
    }
}