using System.Threading.Tasks;
using OrderClaptrap.Models.AuctionItem;
using Newbe.Claptrap;
using Newbe.Claptrap.StorageProvider.Relational.StateStore;

namespace OrderClaptrap.Actors.AuctionItem
{
    public class AuctionItemActorStateLoader : DecoratedStateLoader
    {
        public AuctionItemActorStateLoader(IStateLoader stateLoader) : base(stateLoader)
        {
        }

        public override async Task<IState?> GetStateSnapshotAsync()
        {
            var state = await StateLoader.GetStateSnapshotAsync();
            if (state == null)
            {
                return null;
            }

            var itemState = (AuctionItemState) state.Data;
            var records = itemState.BiddingRecords;
            itemState.InitBiddingRecords();
            if (records != null)
            {
                foreach (var (key, value) in records)
                {
                    itemState.BiddingRecords.Add(key, value);
                }
            }

            return state;
        }
    }
}