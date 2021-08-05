using Newbe.Claptrap;
using OrderClaptrap.Models.Accounts;
using OrderClaptrap.Models.Accounts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClaptrap.Actors.Accounts.Events
{
    internal class TransferAccountCompletedEventHandler : NormalEventHandler<TransferAccountState, TransferAccountCompletedEvent>
    {
        private IClock _clock;

        public TransferAccountCompletedEventHandler(IClock clock)
        {
            _clock = clock;
        }

        public override ValueTask HandleEvent(TransferAccountState stateData, TransferAccountCompletedEvent eventData, IEventContext eventContext)
        {
            if (stateData.TransferAccountInRecords == null)
            {
                stateData.InitTransferAccountInRecords();
            }

            stateData.AddBalance(eventData.Amount);
            var records = stateData.TransferAccountInRecords;

            var time = _clock.UtcNow.ToLocalTime();
            records.Add(time, new TransferAccountInRecord
            {
                Amount = eventData.Amount,
                DateTime = time,
                RemoteAccountId = eventData.RemoteAccountId
            });
            stateData.TransferAccountInRecords = records;
            return ValueTask.CompletedTask;
        }
    }
}