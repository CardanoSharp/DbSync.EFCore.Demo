using CardanoSharp.DbSync.EntityFramework;
using CardanoSharp.DbSync.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationIntegrationTests.Builders
{
    public static class TransactionBuilder
    {
        public static void GenerateBlocks(int numberOfBlocks, CardanoContext cardanoContext)
        {

            for (int i = 1; i <= numberOfBlocks; i++)
            {
                var block = new Tx
                {
                    Hash = i,
                    Block = new Block
                    {
                        SlotNo = i + 1,
                        EpochNo = i + 2,
                        Time = DateTime.UtcNow,

                    },
                    Fee = i + 3,
                    OutSum = i + 4,
                    TxOut
                };
                cardanoContext.Add(block);
                cardanoContext.SaveChangesAsync();

            }

        }

    }
}
}
