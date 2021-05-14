using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.BlockchainTransactions
{
    public static class GetTransactionInformation
    {
        /// <summary>
        /// Command to query for transation data by hash
        /// </summary>
        public record GetTransactionDataFromHashCommand(string Identifier) : IRequest<GetTransactionDataResponse>;

        /// <summary>
        /// Command to query for transation data by hash
        /// </summary>
        public record GetTransactionDataFromIdCommand(long Id) : IRequest<GetTransactionDataResponse>;

        /// <summary>
        /// Passes in the transaction id to query the blockchain to get a specfic transaction and return transaction data found in the Transaction data response.
        /// </summary>
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

        /// <summary>
        /// Passes in the transaction hash to query the blockchain to get a specfic transaction and return transaction data found in the Transaction data response.
        /// </summary>
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
       
        /// <summary>
        /// The respone returned by the two handlers to display the transaction data. Includes the hash, slot number, what epoch the transactions occured, the time transaction occured, 
        /// the fee of the transaction, the total output sum, and all associated addresses that were used in the transaction.
        /// </summary>
        public record GetTransactionDataResponse(string Hash, int? SlotNo, int? EpochNo, DateTime Time, decimal Fee, decimal OutSum, List<string> TxInAddress, List<string> StakeAddressIn,  List<string> TxOutAddress, string MetaData);
    }
}
