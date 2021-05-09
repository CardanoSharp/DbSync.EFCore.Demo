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

        public async Task<List<TransactionsInEpochResponse>> GetTransactionsForUserEnteredEpoch(int epoch)

        {
            // TODO Rework the query
            List<TransactionsInEpochResponse> returnList = new();

            var blocksInEpoch = await _cardanoContext.Blocks
                .Where(x => x.EpochNo == epoch)
                .Include(x => x.Txes)
                .ToListAsync();

            foreach (var block in blocksInEpoch)
            {
                foreach (var tx in block.Txes)
                {
                    returnList.Add(new TransactionsInEpochResponse(tx.Id, tx.Size, tx.Hash, tx.Fee));
                }
            }

            return returnList;
        }

        public async Task<GetTransactionDataResponse> GetTransactionDataDetailsFromHash(string hash)
        {
            

            //TODO encode the string value to match that of the postgres DB to make an accurate query to retrieve data based on hash

            var test = _cardanoContext.Txes.FromSqlRaw($"select * from public.tx t where encode(hash, 'hex') =  '{hash}'",hash).FirstOrDefault(); 


            var transactionDetails = await _cardanoContext.Txes.Where(s => s.Hash == test.Hash)
                                .Include(s => s.Block)
                                .Include(s => s.TxOuts)
                                .Include(s => s.TxMetadata)
                                .Include(s => s.TxInTxInNavigations)
                                .ThenInclude(s => s.TxOut)
                                .ThenInclude(s => s.TxOuts)
                                .FirstOrDefaultAsync();

            return new GetTransactionDataResponse(transactionDetails.Hash.ToString(), transactionDetails.Block.SlotNo.Value, transactionDetails.Block.EpochNo.Value,
                                                  transactionDetails.Block.Time, transactionDetails.Fee, transactionDetails.OutSum, null, transactionDetails.TxOuts.Select(s => s.Address).ToList(), transactionDetails.TxMetadata.Select(s => s.Json).FirstOrDefault());
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



            return new GetTransactionDataResponse(Encoding.UTF7.GetString(transactionDetails.Hash), transactionDetails.Block.SlotNo, transactionDetails.Block.EpochNo,
                                                  transactionDetails.Block.Time, transactionDetails.Fee, transactionDetails.OutSum, new List<string>(), transactionDetails.TxOuts.Select(s => s.Address).ToList(), transactionDetails.TxMetadata.Select(s => s.Json).FirstOrDefault());
        }

    }
}