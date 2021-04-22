using System.Collections.Generic;
using System.Threading.Tasks;
using static Application.BlockChainTransactions.UserEnteredTransactionsPerEpoch;
using static Application.EpochData.GetCurrentEpoch;

namespace Application.Common.Interfaces
{
    public interface IQueries
    {
        int GetBlockInformation(int slotNumber);

        Task<List<TransactionsInEpochResponse>> GetTransactionsPerEpochAsync(int epoch);

        Task<GetCurrentEpochResponse> GetCurrentEpoch(); 
    }
}