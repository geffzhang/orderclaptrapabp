using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderClaptrap.EntityFrameworkCore.Entities
{
    public class Product : FullAuditedAggregateRoot<Guid>, IHasExtraProperties
    {
        /// <summary>
        /// 基础价格
        /// </summary>
        public decimal BasePrice { get; set; }

        public Product()
        {
        }

        public Product(Guid id, decimal basePrice) : base(id)
        {
            BasePrice = basePrice;
        }
    }
}