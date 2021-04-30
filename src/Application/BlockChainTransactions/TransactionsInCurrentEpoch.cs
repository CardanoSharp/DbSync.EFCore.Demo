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
        public record TransactionsInCurrentEpochCommand() : IRequest<List<TransactionsInEpochResponse>>;
        public class TransactionsInCurrentEpochHandler : IRequestHandler<TransactionsInCurrentEpochCommand, List<TransactionsInEpochResponse>>
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

            public async Task<List<TransactionsInEpochResponse>> Handle(TransactionsInCurrentEpochCommand request, CancellationToken cancellationToken)
            {
                var currentEpoch = await _mediator.Send(new GetCurrentEpochCommand(), cancellationToken);
                var transactions =  await _context.GetTransactionsForUserEnteredEpoch(currentEpoch.CurrentEpoch);
                _logger.LogInformation("The system returned a current Epoch of {CurrentEpoch} and transaction in that Epoch are {transactions} all done at {Time}", currentEpoch, transactions, DateTime.UtcNow);
                return transactions; 
                
            }
        }
    }
}
