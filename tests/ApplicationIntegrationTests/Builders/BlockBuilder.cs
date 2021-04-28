using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using CardanoSharp.DbSync.EntityFramework;
using CardanoSharp.DbSync.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationIntegrationTests.Builders
{
    public class BlockBuilder
    {
        public static void GenerateBlocks(int numberOfBlocks)
        {
            using (var _cardanoContext = CreateNewContextOptions())
            {
                var random = new Randomizer();

                for (int i = 0; i < numberOfBlocks; i++)
                {
                    var block = new Block
                    {
                        EpochNo = i,
                        TxCount = random.Number(1, 1000),
                    };
                    _cardanoContext.Add(block);

                }
                _cardanoContext.SaveChangesAsync();
            }

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
}
