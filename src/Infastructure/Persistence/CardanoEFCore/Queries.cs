using Application.Common.Interfaces;
using CardanoSharp.DbSync.EntityFramework;
using CardanoSharp.DbSync.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public int GetTransactionsPerEpoch(int epoch)
        {
            int numberOfTransactions = 0;

            var txListInEpoch = _cardanoContext.Txes.AsEnumerable().GroupBy(s => s.Block.EpochNo == epoch);

            foreach(Tx tx in txListInEpoch)
            {
                numberOfTransactions = +1;
            }

            return numberOfTransactions; 

        }
    }
}