using CardanoSharp.DbSync.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;
using System.Threading.Tasks;
using ApplicationIntegrationTests.Builders;
using System.Linq;
using System.Text;

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
            BlockBuilder.GenerateBlocks(30, _cardanoContext);

            //act
            var firstBlock = await _cardanoContext.Blocks.FirstOrDefaultAsync();

            //assert
            Assert.True(firstBlock.EpochNo is 1);
            
        }

        [Fact]
        public async void GetCurrentEpochTest()
        {
            BlockBuilder.GenerateBlocks(30, _cardanoContext);

            var currentEpoch = await _cardanoContext.Blocks
                        .MaxAsync(s => s.EpochNo);

            Assert.Equal(30, currentEpoch.Value); 
        }
        
        [Theory]
        [InlineData(5)]
        [InlineData(20)]
        public async void GetTransactionsInEpochTest(int expected)
        {
            BlockBuilder.GenerateBlocks(30, _cardanoContext);

            var txCount = await _cardanoContext.Blocks
                .Where(x => x.EpochNo == expected - 1).FirstOrDefaultAsync(); 
            
            Assert.Equal(expected * 5, txCount.TxCount + 5);

        }

        [Theory]
        [InlineData("1")]
        [InlineData("5")]
        [InlineData("10")]
        public async void GetTransactionDetailsTest(string iD)
        {
            var encoding = Encoding.ASCII;

            TransactionBuilder.GenerateTransactions(10, _cardanoContext, encoding);

            var txHashes = _cardanoContext.Txes
                .Where(t => t.Hash != null)
                .Select(s => encoding.GetString(s.Hash).ToLower().Replace("-", "")).ToList();

            var transactionDetails = await _cardanoContext.Txes
                .Where(s => s.Hash != null)
                .Include(s => s.Block)
                .Include(s => s.TxOuts)
                .Include(s => s.TxMetadata)
                .Include(s => s.TxInTxInNavigations)
                .ThenInclude(s => s.TxOut)
                .ThenInclude(s => s.TxOuts)
                .FirstOrDefaultAsync(s => encoding.GetString(s.Hash).ToLower().Replace("-", "") == iD);

            Assert.Equal(Encoding.ASCII.GetBytes(iD), transactionDetails.Hash);
            Assert.Equal(Int32.Parse(iD) + 1, transactionDetails.Block.SlotNo);
            Assert.Equal(Int32.Parse(iD) + 2, transactionDetails.Block.EpochNo);
            Assert.Equal(Int32.Parse(iD) + 3, transactionDetails.Fee);
            Assert.Equal(Int32.Parse(iD) + 4, transactionDetails.OutSum);
            Assert.Equal("TxOutAddress" + iD.ToString(), transactionDetails.TxOuts.Select(s => s.Address).FirstOrDefault());
            Assert.Equal("Hello There Friends From index : " + iD.ToString(), transactionDetails.TxMetadata.Select(s => s.Json).FirstOrDefault());
            //Assert.Equal("TxOutAddress" + iD.ToString(), transactionDetails.TxInTxInNavigations.Select(s => s.TxOut.TxOuts.Select(s => s.Address)).FirstOrDefault().ToString()); 
        }

        public void Dispose()
        {
            _cardanoContext.Dispose(); 
        }
    }
}
