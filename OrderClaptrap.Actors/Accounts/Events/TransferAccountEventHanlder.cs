using Newbe.Claptrap;
using OrderClaptrap.EntityFrameworkCore.Entities;
using OrderClaptrap.Models.Accounts;
using OrderClaptrap.Models.Accounts.Events;
using System.Threading.Tasks;

namespace OrderClaptrap.Actors.Accounts.Events
{
    public class TransferAccountEventHanlder : NormalEventHandler<TransferAccountState, TransferAccountEvent>
    {
        private IAccountRepository _repository;
        private IClock _clock;

        public TransferAccountEventHanlder(IAccountRepository repository, IClock clock)
        {
            _repository = repository;
            _clock = clock;
        }

        public override ValueTask HandleEvent(TransferAccountState stateData, TransferAccountEvent eventData, IEventContext eventContext)
        {
            if (stateData.TransferAccountOutRecords == null)
            {
                stateData.InitTransferAccountOutRecords();
            }

            stateData.SubBalance(eventData.Amount);
            var records = stateData.TransferAccountOutRecords;

            var time = _clock.UtcNow.ToLocalTime();
            records.Add(time, new TransferAccountOutRecord
            {
                Amount = eventData.Amount,
                DateTime = time,
                RemoteAccountId = eventData.RemoteAccountId
            });
            stateData.TransferAccountOutRecords = records;
            return ValueTask.CompletedTask;
        }
    }
}