using Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Application.BlockChainTransactions.UserEnteredTransactionsPerEpoch;

namespace Application.BlockChainTransactions
{
    public static class TransactionsInCurrentEpoch
    {
        public record TransactionsInCurrentEpochCommand() : IRequest<List<TransactionsInEpochResponse>>;
        public class TransactionsInCurrentEpochHandler : IRequestHandler<TransactionsInCurrentEpochCommand, List<TransactionsInEpochResponse>>
        {
            private readonly IQueries _context;

            public TransactionsInCurrentEpochHandler(IQueries context)
            {
                _context = context;
            }

            public async Task<List<TransactionsInEpochResponse>> Handle(TransactionsInCurrentEpochCommand request, CancellationToken cancellationToken)
            {
                return await _context.GetTransactionsPerEpochAsync(4);
            }
        }
    }
}
