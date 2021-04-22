using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.BlockChainTransactions
{
    public static class UserEnteredTransactionsPerEpoch
    {
        public record UserEnteredEochCommand(int Epoch) : IRequest<List<TransactionsInEpochResponse>>;
        public class UserEnteredTransactionsInEpochHandler : IRequestHandler<UserEnteredEochCommand, List<TransactionsInEpochResponse>>
        {
            private readonly IQueries _context;

            public UserEnteredTransactionsInEpochHandler(IQueries context)
            {
                _context = context;
            }

            public async Task<List<TransactionsInEpochResponse>> Handle(UserEnteredEochCommand request, CancellationToken cancellationToken)
            {
                return await _context.GetTransactionsPerEpochAsync(request.Epoch);
            }
        }

        public record TransactionsInEpochResponse(long Id, int Size, byte[] Hash, decimal Fee);
    }
}
