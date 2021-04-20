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
    public static class TransactionsPerEpoch
    {
        public record UserEnteredEochCommand(int Epoch) : IRequest<List<TransactionsInEpoch>>;

        public class Handler : IRequestHandler<UserEnteredEochCommand, List<TransactionsInEpoch>>
        {
            private readonly IQueries _context;

            public Handler(IQueries context)
            {
                _context = context;
            }            

            public async Task<List<TransactionsInEpoch>> Handle(UserEnteredEochCommand request, CancellationToken cancellationToken)
            {
                return await _context.GetTransactionsPerEpochAsync(request.Epoch);
            }
        }

        public record TransactionsInEpoch(long Id, int Size, byte[] Hash, decimal Fee ); 
    }
}
