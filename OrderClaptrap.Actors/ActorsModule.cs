using Autofac;
using OrderClaptrap.Actors.AuctionItem;

namespace OrderClaptrap.Actors
{
    public class ActorsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<AuctionItemActorStateLoader>()
                .AsSelf();
        }
    }
}