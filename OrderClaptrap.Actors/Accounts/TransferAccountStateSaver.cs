using Newbe.Claptrap;
using Newbe.Claptrap.StorageProvider.Relational.StateStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClaptrap.Actors.Accounts
{
    public class TransferAccountStateSaver : DecoratedStateSaver
    {
        public TransferAccountStateSaver(IStateSaver stateSaver) : base(stateSaver)
        {
        }

        public override Task SaveAsync(IState state)
        {
            throw new NotImplementedException();
        }
    }
}