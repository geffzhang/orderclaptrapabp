using Dapr.Actors.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newbe.Claptrap.Dapr;
using OrderClaptrap.EntityFrameworkCore.Entities;
using OrderClaptrap.IActor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Users;

namespace OrderClaptrap.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IActorProxyFactory _actorProxyFactory;

        public AccountController(ICurrentUser currentUser,
            IActorProxyFactory actorProxyFactory,
            IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _actorProxyFactory = actorProxyFactory;
            _currentUser = currentUser;
        }

        [HttpGet()]
        [Route("~/GetAccount")]
        public async Task<IActionResult> GetAccount()
        {
            var account = await _accountRepository.FindAsync(a => a.Id == _currentUser.GetId());
            if (account == null)
            {
                account = new Account(_currentUser.GetId(), _currentUser.UserName);
                await _accountRepository.InsertAsync(account);
            }
            return Ok(account);
        }

        [HttpGet()]
        [Route("~/GetCurrentInfo")]
        public async Task<IActionResult> GetCurrentInfo()
        {
            var info = new
            {
                Name = _currentUser.Name,
                SurName = _currentUser.SurName,
                Email = _currentUser.Email,
                Role = _currentUser.Roles,
                UserName = _currentUser.UserName,
                PhoneNumber = _currentUser.PhoneNumber,
                TenantId = _currentUser.TenantId
            };
            return Ok(info);
        }

        [HttpGet()]
        [Route("~/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _accountRepository.GetListAsync();

            return Ok(result);
        }

        [HttpPost()]
        [Route("~/TransferTo")]
        public async Task<IActionResult> TransferTo(TryTransferAccountInput input)
        {
            input.AccountId = _currentUser.GetId();

            var actor = _actorProxyFactory.GetClaptrap<ITransferAccountActor>(_currentUser.GetId().ToString());

            var result = await actor.TryTransferOutAccount(input);

            return Ok(result);
        }
    }
}