using System.Collections.Generic;
using static Application.BlockChainTransactions.TransactionsPerEpoch;

namespace Application.Common.Interfaces
{
    public interface IQueries
    {
        int GetBlockInformation(int slotNumber);

        List<Response> GetTransactionsPerEpoch(int epoch); 
    }
}