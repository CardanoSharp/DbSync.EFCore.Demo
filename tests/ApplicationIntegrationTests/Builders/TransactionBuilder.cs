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
        public static void GenerateTransactions(int numberOfTransactions, CardanoContext cardanoContext, Encoding encoding)
        {
            for (int i = 1; i <= numberOfTransactions; i++)
            {
                var transaction = new Tx
                {
                    Hash = encoding.GetBytes(i.ToString()),
                    Block = new Block
                    {
                        SlotNo = i + 1,
                        EpochNo = i + 2,
                        Time = DateTime.UtcNow,

                    },
                    Fee = i + 3,
                    OutSum = i + 4,
                    TxOuts = new List<TxOut>() {
                        new TxOut
                        {
                            Address = "TxOutAddress" + i.ToString()
                        }
                    },

                    TxMetadata = new List<TxMetadatum>() {
                        new TxMetadatum {
                            Json = "Hello There Friends From index : " + i.ToString(),
                        }
                    },

                    TxInTxInNavigations = new List<TxIn>()
                    {
                        new TxIn
                        {
                            TxOut = new Tx
                            {
                                TxOuts = new List<TxOut> 
                                {
                                    new TxOut 
                                    {
                                        Address = "TxInAddress" + i.ToString()
                                    }
                                }
                            },

                         }
                    },

                };

                cardanoContext.Add(transaction);
                cardanoContext.SaveChangesAsync();

            }

        }

    }
}

