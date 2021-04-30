using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using CardanoSharp.DbSync.EntityFramework;
using CardanoSharp.DbSync.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationIntegrationTests.Builders
{
    public class BlockBuilder
    {
        public static void GenerateBlocks(int numberOfBlocks, CardanoContext cardanoContext)
        {

            for (int i = 0; i < numberOfBlocks; i++)
            {
                var block = new Block
                {
                    EpochNo = i,
                    TxCount = (long)i * 5,
                };
                cardanoContext.Add(block);

            }
            cardanoContext.SaveChangesAsync();
        }

    }
}
