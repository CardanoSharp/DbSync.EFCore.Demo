using Application.Common.Interfaces;
using CardanoSharp.DbSync.EntityFramework;
using System.Linq;

namespace Infastructure.Persistence
{
    public class Queries : IQueries
    {
        protected CardanoContext cardanoContext = new CardanoContext();

        public int GetBlockInformation(int slotNumber)
        {
            return (int)cardanoContext.Blocks.Where(s => s.EpochSlotNo == slotNumber).Select(s => s.BlockNo).FirstOrDefault();
        }

    }
}