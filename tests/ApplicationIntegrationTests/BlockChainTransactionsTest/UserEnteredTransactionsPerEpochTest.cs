using Application.Common.Interfaces;
using ApplicationIntegrationTests.Builders;
using CardanoSharp.DbSync.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Application.BlockChainTransactions.TransactionsPerEpoch;

namespace ApplicationIntegrationTests.BlockChainTransactionsTest
{
    public class UserEnteredTransactionsPerEpochTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        public void GetUserEpochTest(int userEpoch)
        {

            BlockBuilder.GenerateBlocks(1000);
            var mock = new Mock<IQueries>();
            List<TransactionsInEpochResponse> ListOfResponses = new List<TransactionsInEpochResponse>();
            
            using(var _cardanoContext = CreateNewContextOptions())
            {
                mock.Setup(query => query.GetTransactionsForUserEnteredEpoch(userEpoch)).ReturnsAsync(ListOfResponses);
            }
            


            Assert.Equal(100, ListOfResponses.Count);
        }

        private static CardanoContext CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var builder = new ConfigurationBuilder()
                        .Build();

            var options = new DbContextOptionsBuilder<CardanoContext>()
                      .UseInMemoryDatabase(databaseName: "Cardano")
                      .Options;

            return new CardanoContext(options, builder);
        }
}
