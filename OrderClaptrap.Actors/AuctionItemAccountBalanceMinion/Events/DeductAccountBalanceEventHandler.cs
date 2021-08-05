using Dapr.Actors.Client;
using Newbe.Claptrap;
using Newbe.Claptrap.Dapr;
using OrderClaptrap.EntityFrameworkCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClaptrap.Actors.AuctionItemAccountBalanceMinion.Events
{
    //public class DeductAccountBalanceEventHandler : NormalEventHandler<AuctionItemAccountBalanceState, NewBidderEvent>
    //{
    //    private readonly IAccountRepository _accountRepository;
    //    private readonly IActorProxyFactory _actorProxyFactory;

    //    public DeductAccountBalanceEventHandler(IAccountRepository accountRepository,
    //        IActorProxyFactory actorProxyFactory)
    //    {
    //        _accountRepository = accountRepository;
    //        _actorProxyFactory = actorProxyFactory;
    //    }

    //    public override async ValueTask HandleEvent(AuctionItemAccountBalanceState stateData, NewBidderEvent eventData, IEventContext eventContext)
    //    {
    //        var account = await _accountRepository.FindAsync(a => a.Id == eventData.UserId);
    //        stateData.BalanceTime = DateTime.Now;
    //        stateData.BeforBalance = account.Balance;
    //        stateData.AfterBalance = account.SubBalance(eventData.Price);
    //        stateData.AccountId = account.Id;
    //        await _accountRepository.UpdateAsync(account);
    //    }
    //}
}