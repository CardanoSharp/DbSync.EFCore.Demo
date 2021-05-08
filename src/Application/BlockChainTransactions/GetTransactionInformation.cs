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
        public record GetTransactionDataFromHashCommand(string Identifier) : IRequest<GetTransactionDataResponse>;

        public record GetTransactionDataFromIdCommand(long Id) : IRequest<GetTransactionDataResponse>;

        public class GetTransactionDataFromIdHandler : IRequestHandler<GetTransactionDataFromIdCommand, GetTransactionDataResponse>
        {
            private readonly IQueries _context;
            private readonly ILogger<GetTransactionDataResponse> _logger;

            public GetTransactionDataFromIdHandler(IQueries context, ILogger<GetTransactionDataResponse> logger)
            {
                _context = context;
                _logger = logger;
            }


            public async Task<GetTransactionDataResponse> Handle(GetTransactionDataFromIdCommand request, CancellationToken cancellationToken)
            {

                var transactionDetails = await _context.GetTransactionDataDetailsFromId(request.Id);

                _logger.LogInformation("The user requested transaction {id } at {time} and {transaction} was returned", request.Id, DateTime.UtcNow, transactionDetails);

                return transactionDetails; 
            }
        }

        public class GetTransactionDataFromHashHandler : IRequestHandler<GetTransactionDataFromHashCommand, GetTransactionDataResponse>
        {
            private readonly IQueries _context;
            private readonly ILogger<GetTransactionDataResponse> _logger;

            public GetTransactionDataFromHashHandler(IQueries context, ILogger<GetTransactionDataResponse> logger)
            {
                _context = context;
                _logger = logger;
            }


            public async Task<GetTransactionDataResponse> Handle(GetTransactionDataFromHashCommand request, CancellationToken cancellationToken)
            {

                var transactionDetails = await _context.GetTransactionDataDetailsFromHash(request.Identifier);

                _logger.LogInformation("The user requested transaction {id } at {time} and {transaction} was returned", request.Identifier, DateTime.UtcNow, transactionDetails);

                return transactionDetails;
            }
        }
        public record GetTransactionDataResponse(byte[] Hash, int? SlotNo, int? EpochNo, DateTime Time, decimal Fee, decimal OutSum, List<string> TxInAddress, List<string> TxOutAddress, string MetaData);
    }
}
