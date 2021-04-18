using System.Collections.Generic;
using System.Threading.Tasks;
using static Application.BlockChainTransactions.TransactionsPerEpoch;

namespace Application.Common.Interfaces
{
    public interface IQueries
    {
        int GetBlockInformation(int slotNumber);

        Task<List<Response>> GetTransactionsPerEpochAsync(int epoch);
    }
}