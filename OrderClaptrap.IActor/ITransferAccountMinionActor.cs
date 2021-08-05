using Newbe.Claptrap;
using Newbe.Claptrap.Dapr.Core;
using OrderClaptrap.Models;
using OrderClaptrap.Models.Accounts;
using OrderClaptrap.Models.TransferAccountMinion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClaptrap.IActor
{
    [ClaptrapMinion(ClaptrapCodes.TransferAccountActor)]
    [ClaptrapState(typeof(NoneStateData), ClaptrapCodes.TransferAccountMinionActor)]
    public interface ITransferAccountMinionActor : IClaptrapMinionActor
    {
    }
}