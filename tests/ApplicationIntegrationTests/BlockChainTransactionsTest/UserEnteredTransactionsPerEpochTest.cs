using ApplicationIntegrationTests.Builders;
using System.Linq;
using Xunit;

namespace ApplicationIntegrationTests.BlockChainTransactionsTest
{
    public class UserEnteredTransactionsPerEpochTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        public void GetUserEpochTest(int userEpoch)
        {
             BlockBuilder.GenerateBlocks(15);

            var epoch =   BlockBuilder.BlockList.Where(s => s.EpochNo == userEpoch).FirstOrDefault();

            Assert.Equal(userEpoch, epoch.EpochNo);
        }
    }
}
