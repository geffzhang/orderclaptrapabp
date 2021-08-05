using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace OrderClaptrap.EntityFrameworkCore.EntityFrameworkCore
{
    public class OrderClaptrapModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public OrderClaptrapModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {
        }
    }
}