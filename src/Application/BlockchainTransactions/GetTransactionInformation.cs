using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
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
        public record GetTransactionDataCommand(string Identifier) : IRequest<GetTransactionDataResponse>;

        public class GetTransactionDataHandler : IRequestHandler<GetTransactionDataCommand, GetTransactionDataResponse>
        {
            private readonly IQueries _context;
            private readonly ILogger<GetTransactionDataResponse> _logger;

            public GetTransactionDataHandler(IQueries context, ILogger<GetTransactionDataResponse> logger)
            {
                _context = context;
                _logger = logger;
            }


            public async Task<GetTransactionDataResponse> Handle(GetTransactionDataCommand request, CancellationToken cancellationToken)
            {

                var transactionDetails = await _context.GetTransactionDataDetails(request.Identifier);

                _logger.LogInformation("The user requested transaction {id } at {time} and {transaction} was returned", request.Identifier, DateTime.UtcNow, transactionDetails);

                return transactionDetails; 
            }
        }

        public record GetTransactionDataResponse(string Hash, int SlotNo, int EpochNo, DateTime Time, decimal Fee, decimal OutSum, List<string>? TxInAddress, List<string> TxOutAddress, string MetaData);
    }
}
