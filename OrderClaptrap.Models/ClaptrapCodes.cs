namespace OrderClaptrap.Models
{
    public static class ClaptrapCodes
    {
        public const string AuctionItemActor = "auction_claptrap_newbe";
        private const string AuctionItemEventSuffix = "_e_" + AuctionItemActor;
        public const string NewBidderEvent = "newBidder" + AuctionItemEventSuffix;
        public const string AuctionItemUserCountMinionActor = "auction_user_claptrap_newbe";
        public const string AuctionItemAccountBalanceMinionActor = "auction_account_balance_claptrap_newbe";

        public const string TransferAccountActor = "transferaccount_claptrap_newbe";
        private const string AccountEventSuffix = "_e_" + TransferAccountActor;
        public const string TransferAccountMinionActor = "transferaccount_minion_claptrap_newbe";
        public const string TransferAccountEvent = "transferAccount" + AccountEventSuffix;
        public const string TransferAccountCompletedEvent = "transferAccountCompleted" + AccountEventSuffix;
    }
}