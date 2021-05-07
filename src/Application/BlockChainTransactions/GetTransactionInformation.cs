using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.BlockchainTransactions
{
    public static class GetTransactionInformation
    {
        public record GetTransactionDataCommand(string identifier) : IRequest<GetTransactionDataResponse>;

        public class GetTransactionDataHandler : IRequestHandler<GetTransactionDataCommand, GetTransactionDataResponse>
        {
            public Task<GetTransactionDataResponse> Handle(GetTransactionDataCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }

        public record GetTransactionDataResponse(byte[] Hash, int SlotNo, int EpochNo, DateTime Time, decimal Fee, decimal OutSum, long TxIn, long TxOut, string MetaData); 
    }
}
