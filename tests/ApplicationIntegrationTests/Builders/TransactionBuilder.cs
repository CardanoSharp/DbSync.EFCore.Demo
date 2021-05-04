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
                var transaction = new Tx
                {
                    Hash = BitConverter.GetBytes(i),
                    Block = new Block
                    {
                        SlotNo = i + 1,
                        EpochNo = i + 2,
                        Time = DateTime.UtcNow,

                    },
                    Fee = i + 3,
                    OutSum = i + 4,
                    TxOuts = new List<TxOut>(),
                    TxMetadata = new List<TxMetadatum>(),
                };

                transaction.TxMetadata.Add(new TxMetadatum
                {
                    Json = "Hello There Friends at index : " + i.ToString(),
                });

                transaction.TxOuts.Add(new TxOut
                {
                    Address = "Hi" + i.ToString(),

                });

                cardanoContext.Add(transaction);
                cardanoContext.SaveChangesAsync();

            }

        }

    }
}

