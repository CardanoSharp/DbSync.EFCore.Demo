using Application.Common.Interfaces;
using AutoMapper;
using CardanoSharp.DbSync.EntityFramework;
using CardanoSharp.DbSync.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Application.BlockChainTransactions.TransactionsPerEpoch;

namespace Infastructure.Persistence
{
    public class Queries : IQueries
    {
        private readonly CardanoContext _cardanoContext;
        private readonly IMapper _mapper;

        public Queries(CardanoContext cardanoContext, IMapper mapper)
        {
            _cardanoContext = cardanoContext;
            _mapper = mapper;
        }
        public int GetBlockInformation(int slotNumber)
        {
            return (int)_cardanoContext.Blocks.Where(s => s.EpochSlotNo == slotNumber).Select(s => s.BlockNo).FirstOrDefault();
        }

        public async Task<List<Response>> GetTransactionsPerEpochAsync(int epoch)
        {

            // TODO Make the below work. The first one returns the block in an epoch, and the second returns nothing. 
            List<Response> returnList = new();

            var blocksInEpoch = await _cardanoContext.Blocks
                .Where(x => x.EpochNo == epoch)
                .ToListAsync();

            foreach (var block in blocksInEpoch)
            {
                foreach (var tx in block.Txes)
                {
                    returnList.Add(new Response(tx.Id, tx.Size, tx.Hash, tx.Fee)); 




                }
            }

                return returnList;

        }
    }
}