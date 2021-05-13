using Application.Common.Interfaces;
using AutoMapper;
using CardanoSharp.DbSync.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Application.EpochData.GetCurrentEpoch;
using static Application.BlockChainTransactions.TransactionsPerEpoch;
using static Application.BlockchainTransactions.GetTransactionInformation;
using Npgsql;
using System.Text;

namespace Infastructure.Persistence
{
    public class Queries : IQueries
    {
        private readonly CardanoContext _cardanoContext;

        public Queries(CardanoContext cardanoContext)
        {
            _cardanoContext = cardanoContext;
        }
        public int GetBlockInformation(int slotNumber)
        {
            return (int)_cardanoContext.Blocks.Where(s => s.EpochSlotNo == slotNumber).Select(s => s.BlockNo).FirstOrDefault();
        }

        public async Task<GetCurrentEpochResponse> GetCurrentEpoch()
        {
            var currentEpoch = await _cardanoContext.Blocks
                                    .MaxAsync(s => s.EpochNo);

            return new GetCurrentEpochResponse(currentEpoch.Value);

        }

        /// <summary>
        /// This method returns a list of all transaction in an epoch. It Querys the blockchain to include
        /// all blocks that contain a tx and returns the count. 
        /// </summary>
        /// <param name="epoch"></param> The epoch number that is requested
        /// <returns>
        /// A List of TransactionInResponseModels found in the Application Layer
        /// </returns> 
        public async Task<long> GetTransactionsForUserEnteredEpoch(int epoch)

        {
            var blocksInEpoch = await _cardanoContext.Blocks
                .Where(x => x.EpochNo == epoch)
                .Include(x => x.Txes)
                .ToListAsync();

            return blocksInEpoch.Where(s => s.TxCount > 0).Select(s => s.TxCount).ToList().GetRange(0, blocksInEpoch.Where(s => s.TxCount != 0).Count()).Sum();
        }


    

    public async Task<GetTransactionDataResponse> GetTransactionDataDetailsFromHash(string hash)
    {


        //TODO encode the string value to match that of the postgres DB to make an accurate query to retrieve data based on hash

        var txRetrievedFromEncodedHash = _cardanoContext.Txes.FromSqlRaw($"select * from public.tx t where encode(hash, 'hex') =  '{hash}'", hash).FirstOrDefault();


        var transactionDetails = await _cardanoContext.Txes.Where(s => s.Hash == txRetrievedFromEncodedHash.Hash)
                            .Include(s => s.Block)
                            .Include(s => s.TxOuts)
                            .Include(s => s.TxMetadata)
                            .Include(s => s.TxInTxInNavigations)
                            .ThenInclude(s => s.TxOut)
                            .ThenInclude(s => s.TxOuts)
                            .FirstOrDefaultAsync();

        return new GetTransactionDataResponse(transactionDetails.Hash.ToString(), transactionDetails.Block.SlotNo.Value, transactionDetails.Block.EpochNo.Value,
                                              transactionDetails.Block.Time, transactionDetails.Fee, transactionDetails.OutSum,
                                              transactionDetails.TxInTxInNavigations.SelectMany(s => s.TxOut.TxOuts).Select(s => s.Address).ToList(),
                                              transactionDetails.TxInTxInNavigations.SelectMany(s => s.TxOut.StakeAddresses).SelectMany(s => s.TxOuts).Select(s => s.Address).ToList(),
                                              transactionDetails.TxOuts.Select(s => s.Address).ToList(),
                                              transactionDetails.TxMetadata.Select(s => s.Json).FirstOrDefault());
    }

    public async Task<GetTransactionDataResponse> GetTransactionDataDetailsFromId(long id)
    {
        var transactionDetails = await _cardanoContext.Txes.Where(s => s.Id == id)
                            .Include(s => s.Block)
                            .Include(s => s.TxOuts)
                            .Include(s => s.TxMetadata)
                            .Include(s => s.TxInTxInNavigations)
                            .ThenInclude(s => s.TxOut)
                            .ThenInclude(s => s.TxOuts)
                            .FirstOrDefaultAsync();



        return new GetTransactionDataResponse(transactionDetails.Hash.ToString(), transactionDetails.Block.SlotNo.Value, transactionDetails.Block.EpochNo.Value,
                                   transactionDetails.Block.Time, transactionDetails.Fee, transactionDetails.OutSum,
                                   transactionDetails.TxInTxInNavigations.SelectMany(s => s.TxOut.TxOuts).Select(s => s.Address).ToList(),
                                   transactionDetails.TxInTxInNavigations.SelectMany(s => s.TxOut.StakeAddresses).SelectMany(s => s.TxOuts).Select(s => s.Address).ToList(),
                                   transactionDetails.TxOuts.Select(s => s.Address).ToList(),
                                   transactionDetails.TxMetadata.Select(s => s.Json).FirstOrDefault());
    }

}
}