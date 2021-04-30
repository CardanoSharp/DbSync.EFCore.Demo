using CardanoSharp.DbSync.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CardanoSharp.DbSync.EntityFramework.Models;
using Moq;
using Npgsql;
using System;
using System.IO;
using WebUI;
using Xunit;
using System.Threading.Tasks;
using ApplicationIntegrationTests.Builders;
using System.Linq;

namespace ApplicationIntegrationTests
{
    public class DatabaseFixture : IDisposable
    {
        private readonly CardanoContext _cardanoContext;


        public DatabaseFixture()
        {
            var builder = new ConfigurationBuilder()
                        .Build();

            var options = new DbContextOptionsBuilder<CardanoContext>()
                      .UseInMemoryDatabase(databaseName: "Cardano")
                      .Options;

            _cardanoContext = new CardanoContext(options, builder);
        }

        [Fact]
        public async Task FirstTimeSetUp()
        {
            //arrange
            var block = new Block()
            {
                EpochNo = 1
            };

            _cardanoContext.Blocks.Add(block);
            await _cardanoContext.SaveChangesAsync();

            //act
            var firstBlock = await _cardanoContext.Blocks.FirstOrDefaultAsync();

            //assert
            Assert.True(firstBlock.EpochNo is 1);
            _cardanoContext.Database.EnsureDeleted();
        }

        [Theory]
        [InlineData(10)]
        [InlineData(5)]
        [InlineData(20)]
        public async void GetCurrentEpochTest(int expected)
        {
            BlockBuilder.GenerateBlocks(expected, _cardanoContext);

            var currentEpoch = await _cardanoContext.Blocks
                        .MaxAsync(s => s.EpochNo);

            Assert.Equal(expected, currentEpoch.Value + 1);
            
            _cardanoContext.Database.EnsureDeleted();
        }

        [Theory]
        [InlineData(10)]
        [InlineData(5)]
        [InlineData(20)]
        public async void GetTransactionsInEpochTest(int expected)
        {
            BlockBuilder.GenerateBlocks(expected, _cardanoContext);

            var txCount = await _cardanoContext.Blocks
                .Where(x => x.EpochNo == expected)
                .Include(x => x.TxCount)
                .ToListAsync(); 

            Assert.Equal((long)expected * 5, txCount.Count());

            _cardanoContext.Database.EnsureDeleted();
        }

        public async void Dispose()
        {
            await _cardanoContext.DisposeAsync();
        }
    }
}
