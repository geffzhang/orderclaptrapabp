using System.Threading.Tasks;
using Dapr.Actors.Client;
using OrderClaptrap.IActor;
using Microsoft.AspNetCore.Mvc;
using Newbe.Claptrap.Dapr;
using Volo.Abp.Users;
using System;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace OrderClaptrap.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuctionItemsController : ControllerBase
    {
        private readonly IActorProxyFactory _actorProxyFactory;
        private readonly ICurrentUser _currentUser;

        public AuctionItemsController(
            IActorProxyFactory actorProxyFactory,
            ICurrentUser currentUser)
        {
            _actorProxyFactory = actorProxyFactory;
            _currentUser = currentUser;
        }

        [HttpGet("{itemId:int}/status")]
        public async Task<IActionResult> GetStatus(int itemId = 1)
        {
            var auctionItemActor = _actorProxyFactory.GetClaptrap<IAuctionItemActor>(itemId.ToString());
            var status = await auctionItemActor.GetStatusAsync();
            var result = new
            {
                status
            };
            return Ok(result);
        }

        [HttpGet("{itemId:int}/topPrice")]
        public async Task<IActionResult> GetTopPrice(int itemId = 1)
        {
            var auctionItemActor = _actorProxyFactory.GetClaptrap<IAuctionItemActor>(itemId.ToString());
            var topPrice = await auctionItemActor.GetTopPriceAsync();
            var result = new
            {
                topPrice
            };
            return Ok(result);
        }

        [HttpGet("{itemId:int}")]
        public async Task<IActionResult> GetState(int itemId = 1)
        {
            var auctionItemActor = _actorProxyFactory.GetClaptrap<IAuctionItemActor>(itemId.ToString());
            var state = await auctionItemActor.GetStateAsync();
            var result = new
            {
                state = state
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> TryBidding([FromBody] TryBiddingWebApiInput webApiInput)
        {
            var input = new TryBiddingInput
            {
                Price = webApiInput.Price,
                UserId = webApiInput.UserId,
            };
            var itemId = webApiInput.ItemId;
            var auctionItemActor = _actorProxyFactory.GetClaptrap<IAuctionItemActor>(itemId.ToString());
            var result = await auctionItemActor.TryBidding(input);
            return Ok(result);
        }

        [HttpPost("trybiddingv2")]
        public async Task<IActionResult> TryBiddingV2([FromBody] TryBiddingWebApiInput webApiInput)
        {
            var input = new TryBiddingInput
            {
                Price = webApiInput.Price,
                UserId = _currentUser.GetId()
            };
            var itemId = webApiInput.ItemId;
            var auctionItemActor = _actorProxyFactory.GetClaptrap<IAuctionItemActor>(itemId.ToString());
            var result = await auctionItemActor.TryBidding(input);

            return Ok(result);
        }

        [HttpGet("{itemId:int}/biddingcount")]
        public async Task<IActionResult> GetBiddingCount(int itemId = 1)
        {
            var auctionItemActor = _actorProxyFactory.GetClaptrap<IAuctionItemUserCountMinionActor>(itemId.ToString());
            var result = await auctionItemActor.GetUserBiddingCountAsync();
            return Ok(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetUserName()
        {
            var t2 = new
            {
                Name = _currentUser.FindClaimValue("name"),
                Email = _currentUser.FindClaimValue("email"),
                Role = _currentUser.FindClaimValue("role")
            };
            return Ok(t2);
        }
    }

    public record TryBiddingWebApiInput
    {
        public Guid UserId { get; set; }
        public decimal Price { get; set; }
        public int ItemId { get; set; }
    }
}