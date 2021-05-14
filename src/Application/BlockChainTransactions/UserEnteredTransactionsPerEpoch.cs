using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.BlockChainTransactions
{
    public static class TransactionsPerEpoch
    {
        /// <summary>
        /// The command to get the number of transactions in an epoch that is entered by user
        /// </summary>
        public record UserEnteredEochCommand(int Epoch) : IRequest<TransactionsInEpochResponse>;
        public class UserEnteredTransactionsInEpochHandler : IRequestHandler<UserEnteredEochCommand, TransactionsInEpochResponse>
        {
            private readonly IQueries _context;
            private readonly ILogger<TransactionsInEpochResponse> _logger;

            public UserEnteredTransactionsInEpochHandler(IQueries context, ILogger<TransactionsInEpochResponse> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<TransactionsInEpochResponse> Handle(UserEnteredEochCommand request, CancellationToken cancellationToken)
            {
                var transactions = await _context.GetTransactionsForUserEnteredEpoch(request.Epoch);
                _logger.LogInformation("The user asked for the transactions in Epoch {request} and the system returned {transactions} transactions at {time}", request.Epoch, transactions, DateTime.UtcNow);
                return new TransactionsInEpochResponse(transactions);

            }
        }

        /// <summary>
        /// Response that returns the number of transactions in the entered epoch.
        /// </summary>
        public record TransactionsInEpochResponse(long Transactions);
    }
}
