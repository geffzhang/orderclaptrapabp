using System.Linq;
using System.Threading.Tasks;
using Dapr.Actors.Runtime;
using OrderClaptrap.Actors.AuctionItem.Events;
using OrderClaptrap.IActor;
using OrderClaptrap.Models;
using OrderClaptrap.Models.AuctionItem;
using OrderClaptrap.Models.AuctionItem.Events;
using Newbe.Claptrap;
using Newbe.Claptrap.Dapr;
using Newbe.Claptrap.StorageProvider.Relational.StateStore;
using Dapr.Actors.Client;

namespace OrderClaptrap.Actors.AuctionItem
{
    [Actor(TypeName = ClaptrapCodes.AuctionItemActor)]
    [ClaptrapStateInitialFactoryHandler(typeof(AuctionItemActorInitialStateDataFactory))]
    [ClaptrapStateStore(null, typeof(DecoratedStateLoaderFactory<AuctionItemActorStateLoader>))]
    [ClaptrapEventHandler(typeof(NewBidderEventHandler), ClaptrapCodes.NewBidderEvent)]
    public class AuctionItemActor : ClaptrapBoxActor<AuctionItemState>, IAuctionItemActor
    {
        private readonly IClock _clock;
        private readonly IActorProxyFactory _actorProxyFactory;

        public AuctionItemActor(
            ActorHost actorHost,
            IClaptrapActorCommonService claptrapActorCommonService,
            IActorProxyFactory actorProxyFactory,
            IClock clock) : base(actorHost, claptrapActorCommonService)
        {
            _clock = clock;
            _actorProxyFactory = actorProxyFactory;
        }

        public Task<AuctionItemStatus> GetStatusAsync()
        {
            return Task.FromResult(GetStatusCore());
        }

        private AuctionItemStatus GetStatusCore()
        {
            var now = _clock.UtcNow;
            if (now < StateData.StartTime)
            {
                return AuctionItemStatus.Planned;
            }

            if (now > StateData.StartTime && now < StateData.EndTime)
            {
                return AuctionItemStatus.OnSell;
            }

            return StateData.BiddingRecords?.Any() == true ? AuctionItemStatus.Sold : AuctionItemStatus.UnSold;
        }

        public Task<TryBiddingResult> TryBidding(TryBiddingInput input)
        {
            var status = GetStatusCore();

            if (status != AuctionItemStatus.OnSell)
            {
                return Task.FromResult(CreateResult(false, "It's not on sell"));
            }

            if (input.Price <= GetTopPrice())
            {
                return Task.FromResult(CreateResult(false, "the price error"));
            }

            return HandleCoreAsync();

            async Task<TryBiddingResult> HandleCoreAsync()
            {
                var dataEvent = this.CreateEvent(new NewBidderEvent
                {
                    Price = input.Price,
                    UserId = input.UserId,
                });
                await Claptrap.HandleEventAsync(dataEvent);
                return CreateResult(true);
            }

            TryBiddingResult CreateResult(bool success, string message = default)
            {
                return new()
                {
                    Success = success,
                    NowPrice = GetTopPrice(),
                    UserId = input.UserId,
                    AuctionItemStatus = status,
                    Message = message
                };
            }
        }

        private decimal GetTopPrice()
        {
            return StateData.BiddingRecords?.Any() == true
                ? StateData.BiddingRecords.First().Key
                : StateData.BasePrice;
        }

        public Task<AuctionItemState> GetStateAsync()
        {
            return Task.FromResult(StateData);
        }

        public Task<decimal> GetTopPriceAsync()
        {
            return Task.FromResult(GetTopPrice());
        }
    }
}