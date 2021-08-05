using System.Threading.Tasks;
using OrderClaptrap.Models.AuctionItem;
using OrderClaptrap.Models.AuctionItem.Events;
using Newbe.Claptrap;

namespace OrderClaptrap.Actors.AuctionItem.Events
{
    public class NewBidderEventHandler
        : NormalEventHandler<AuctionItemState, NewBidderEvent>
    {
        private readonly IClock _clock;

        public NewBidderEventHandler(
            IClock clock)
        {
            _clock = clock;
        }

        public override ValueTask HandleEvent(AuctionItemState stateData,
            NewBidderEvent eventData,
            IEventContext eventContext)
        {
            if (stateData.BiddingRecords == null)
            {
                stateData.InitBiddingRecords();
            }

            var records = stateData.BiddingRecords;

            records.Add(eventData.Price, new BiddingRecord
            {
                Price = eventData.Price,
                BiddingTime = _clock.UtcNow.ToLocalTime(),
                UserId = eventData.UserId
            });
            stateData.BiddingRecords = records;
            return ValueTask.CompletedTask;
        }
    }
}