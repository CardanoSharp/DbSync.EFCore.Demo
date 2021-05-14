using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Application.BlockChainTransactions.TransactionsPerEpoch;
using static Application.EpochData.GetCurrentEpoch;

namespace Application.BlockChainTransactions
{
    public static class TransactionsInCurrentEpoch
    {
        /// <summary>
        /// The request to get transactions in the current epoch. 
        /// </summary>
        public record TransactionsInCurrentEpochCommand() : IRequest<TransactionsInEpochResponse>;
        
        /// <summary>
        /// Queries the Cardano database for the current Epoch, and passes that in to the get transactions for current epoch.
        /// </summary>
        /// <param name="request"></param> A void command that triggers the even of querying current epoch.
        /// <param name="cancellationToken"></param>
        /// <returns></returns> Reutnrs a long that represents the transactions in the current epoch. Its uses the same response object that the user entered transactions uses.
        public class TransactionsInCurrentEpochHandler : IRequestHandler<TransactionsInCurrentEpochCommand, TransactionsInEpochResponse>
        {
            private readonly IQueries _context;
            private readonly IMediator _mediator;
            private readonly ILogger<TransactionsInEpochResponse> _logger;

            public TransactionsInCurrentEpochHandler(IQueries context, IMediator mediator, ILogger<TransactionsInEpochResponse> logger)
            {
                _context = context;
                _mediator = mediator;
                _logger = logger;
            }


            public async Task<TransactionsInEpochResponse> Handle(TransactionsInCurrentEpochCommand request, CancellationToken cancellationToken)
            {
                var currentEpoch = await _mediator.Send(new GetCurrentEpochCommand(), cancellationToken);
                var transactions =  await _context.GetTransactionsForUserEnteredEpoch(currentEpoch.CurrentEpoch);
                _logger.LogInformation("The system returned a current Epoch of {CurrentEpoch} and transaction in that Epoch are {transactions} all done at {Time}", currentEpoch, transactions, DateTime.UtcNow);
                return new TransactionsInEpochResponse(transactions); 
                
            }
        }
    }
}
