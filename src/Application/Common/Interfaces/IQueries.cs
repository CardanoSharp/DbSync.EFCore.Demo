﻿using System.Collections.Generic;
using System.Threading.Tasks;
using static Application.BlockChainTransactions.TransactionsPerEpoch;
using static Application.EpochData.GetCurrentEpoch;

namespace Application.Common.Interfaces
{
    public interface IQueries
    {
        int GetBlockInformation(int slotNumber);

        Task<List<TransactionsInEpoch>> GetTransactionsPerEpochAsync(int epoch);

        Task<Respone> GetCurrentEpoch(); 
    }
}