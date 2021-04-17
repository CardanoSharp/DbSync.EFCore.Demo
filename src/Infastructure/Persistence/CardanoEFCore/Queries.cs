using Application.Common.Interfaces;
using AutoMapper;
using CardanoSharp.DbSync.EntityFramework;
using CardanoSharp.DbSync.EntityFramework.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Application.BlockChainTransactions.TransactionsPerEpoch;

namespace Infastructure.Persistence
{
    public class Queries : IQueries
    {
        private readonly CardanoContext _cardanoContext;
        private readonly IMapper _mapper;

        public Queries(CardanoContext cardanoContext, IMapper mapper)
        {
            _cardanoContext = cardanoContext;
            _mapper = mapper;
        }
        public int GetBlockInformation(int slotNumber)
        {
            return (int)_cardanoContext.Blocks.Where(s => s.EpochSlotNo == slotNumber).Select(s => s.BlockNo).FirstOrDefault();
        }

        public List<Response> GetTransactionsPerEpoch(int epoch)
        {
            List<Response> returnList = new List<Response>();

            var txesInEpoch = _cardanoContext.Blocks.AsEnumerable().Where(s => s.EpochNo == epoch).SelectMany(s => s.Txes).ToList(); 

            foreach(var tx in txesInEpoch)
            {
                returnList.Add(_mapper.Map<Response>(tx.TxInTxOuts)); 
            }

            return returnList; 

        }
    }
}