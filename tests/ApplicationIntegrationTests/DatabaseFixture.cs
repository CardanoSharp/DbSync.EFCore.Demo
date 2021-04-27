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

namespace ApplicationIntegrationTests
{
    public class DatabaseFixture : IDisposable
    {
        private readonly CardanoContext cardanoContext;
        private static readonly IConfiguration _configuration;

 
        public DatabaseFixture()
        {
            var builder = new ConfigurationBuilder()
                        .Build();

            var options = new DbContextOptionsBuilder<CardanoContext>()
                      .UseInMemoryDatabase(databaseName: "Cardano")
                      .Options;

            cardanoContext = new CardanoContext(options, builder);
        }

        [Fact]
        public async Task FirstTimeSetUp()
        {
            //arrange
            var block = new Block()
            {
                EpochNo = 1
            };

            cardanoContext.Blocks.Add(block);
            await cardanoContext.SaveChangesAsync();

            //act
            var firstBlock = await cardanoContext.Blocks.FirstOrDefaultAsync();

            //assert
            Assert.True(firstBlock.EpochNo is 1); 
        }


        public async void Dispose()
        {
            await cardanoContext.DisposeAsync();
        }
    }
}
