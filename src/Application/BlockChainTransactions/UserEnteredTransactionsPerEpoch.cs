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

        public record TransactionsInEpochResponse(long Transactions);
    }
}
