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
        public record Command(int Epoch) : IRequest<List<Response>>;

        public class Handler : IRequestHandler<Command, List<Response>>
        {
            private readonly IQueries _context;

            public Handler(IQueries context)
            {
                _context = context;
            }            

            public async Task<List<Response>> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _context.GetTransactionsPerEpochAsync(request.Epoch);
            }
        }

        public record Response(long Id, int Size, byte[] Hash, decimal Fee ); 
    }
}
