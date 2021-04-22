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
                                 .OrderByDescending(s => s.EpochNo)
                                 .FirstOrDefaultAsync();

            int confirmedEpochHasValue = 0;

            if (currentEpoch.EpochNo.HasValue)
            {
                confirmedEpochHasValue = currentEpoch.EpochNo.Value;
            }
            var response = new GetCurrentEpochResponse(confirmedEpochHasValue);

            return response;


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

    }
}