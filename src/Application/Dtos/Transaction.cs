
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BlockChainTransactions
{
    public class Transaction
    {
        public DateTime Time { get; set; }
        public List<int> Epoch { get; set; }
        public List<double> NumberOfTransactions { get; set; }
    }
}
