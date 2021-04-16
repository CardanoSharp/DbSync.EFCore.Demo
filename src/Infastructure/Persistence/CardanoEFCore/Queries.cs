using Application.Common.Interfaces;
using CardanoSharp.DbSync.EntityFramework;
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
            return 1;
        }
    }
}