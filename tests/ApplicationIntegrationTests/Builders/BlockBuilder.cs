using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using CardanoSharp.DbSync.EntityFramework.Models;

namespace ApplicationIntegrationTests.Builders
{
    public static class BlockBuilder
    {
        public static List<Block> BlockList = new List<Block>();

        public static void GenerateBlocks(int numberOfBlocks)
        {
            var random = new Randomizer();

            for (int i = 0; i < numberOfBlocks; i++)
            {
                var block =  new Block
                {
                    EpochNo = i,
                    TxCount = random.Number(1, 1000),
                };

                 BlockList.Add(block); 

            }
        }
    }
}
