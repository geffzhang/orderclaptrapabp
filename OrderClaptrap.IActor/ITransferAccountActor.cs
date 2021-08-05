using Newbe.Claptrap;
using Newbe.Claptrap.Dapr.Core;
using OrderClaptrap.Models;
using OrderClaptrap.Models.Accounts;
using OrderClaptrap.Models.Accounts.Events;
using System;
using System.Threading.Tasks;

namespace OrderClaptrap.IActor
{
    [ClaptrapState(typeof(TransferAccountState), ClaptrapCodes.TransferAccountActor)]
    [ClaptrapEvent(typeof(TransferAccountEvent), ClaptrapCodes.TransferAccountEvent)]
    [ClaptrapEvent(typeof(TransferAccountCompletedEvent), ClaptrapCodes.TransferAccountCompletedEvent)]
    public interface ITransferAccountActor : IClaptrapActor
    {
        Task<TryTransferAccountResult> TryTransferOutAccount(TryTransferAccountInput input);

        Task<TryTransferAccountResult> TryTransferAccount(TryTransferAccountInput input);

        Task<TryTransferAccountResult> TryTransferInAccount(TryTransferAccountInput input);

        Task TransferCompleted();
    }

    public record TryTransferAccountInput
    {
        public Guid AccountId { get; set; }
        public Guid RemoteAccountId { get; set; }

        public decimal Amount { get; set; }
    }
    public record TryTransferAccountResult
    {
        public bool Success { get; set; }
        public Guid UserId { get; set; }

        public decimal NowBalance { get; set; }

        public string Message { get; set; }
    }
}