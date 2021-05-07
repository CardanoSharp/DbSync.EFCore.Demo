using System.Collections.Generic;
using System.Threading.Tasks;
using static Application.BlockchainTransactions.GetTransactionInformation;
using static Application.BlockChainTransactions.TransactionsPerEpoch;
using static Application.EpochData.GetCurrentEpoch;

namespace Application.Common.Interfaces
{
    public interface IQueries
    {
        int GetBlockInformation(int slotNumber);

        Task<List<TransactionsInEpochResponse>> GetTransactionsForUserEnteredEpoch(int epoch);

        Task<GetCurrentEpochResponse> GetCurrentEpoch();

        Task<GetTransactionDataResponse> GetTransactionDataDetails(string id);
    }
}