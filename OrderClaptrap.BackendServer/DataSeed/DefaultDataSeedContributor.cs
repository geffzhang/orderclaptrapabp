using OrderClaptrap.EntityFrameworkCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace OrderClaptrap.BackendServer.DataSeed
{
    public class DefaultDataSeedContributor : IDataSeedContributor,
        ITransientDependency
    {
        protected IAccountRepository _accountRepository { get; }

        protected IProductRepository _productRepository { get; }

        protected IGuidGenerator _guidGenerator { get; }

        public DefaultDataSeedContributor(
            IGuidGenerator guidGenerator,
            IAccountRepository accounts,
            IProductRepository products)
        {
            _accountRepository = accounts;
            _productRepository = products;
            _guidGenerator = guidGenerator;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _productRepository.GetCountAsync() < 10)
            {
                await SeedProduct();
            }
        }

        private async Task SeedProduct()
        {
            var productList = new List<Product>();
            for (int i = 0; i < 10; i++)
            {
                productList.Add(new Product(_guidGenerator.Create(), 10 + i));
            }
            await _productRepository.InsertManyAsync(productList);
        }
    }
}