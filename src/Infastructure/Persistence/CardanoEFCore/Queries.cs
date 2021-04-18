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
            List<Response> returnList = new();

            var txesinepoch = await _cardanoContext.Blocks
                .Where(x => x.EpochNo == epoch)
                .Include(x => x.Txes)
                .ThenInclude(s => s.TxInTxOuts)
                .ToListAsync();

            var test = await _cardanoContext.Txes
                .Where(s => s.BlockId == epoch)
                .Select(s => s.TxInTxOuts)
                .ToListAsync();



            return returnList;

        }
    }
}