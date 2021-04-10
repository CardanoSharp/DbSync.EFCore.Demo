using Application.Common.Interfaces;
using CardanoSharp.DbSync.EntityFramework;
using System.Linq;

namespace Infastructure.Persistence
{
    public class Queries : IQueries
    {
        private CardanoContext cardanoContext = new CardanoContext();

        public int GetBlockInformation(int slotNumber)
        {
            return (int)cardanoContext.Block.Where(s => s.EpochSlotNo == slotNumber).Select(s => s.BlockNo).FirstOrDefault(); 
        }

    }
}
