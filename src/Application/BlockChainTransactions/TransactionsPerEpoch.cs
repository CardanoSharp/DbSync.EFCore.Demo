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
        public record Command(int Epoch) : IRequest<int>;

        public class Handle : IRequestHandler<Command, int>
        {
            private readonly IQueries _context;
            private readonly IMapper _mapper;

            public Handle(IQueries context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

             Task<int> IRequestHandler<Command, int>.Handle(Command request, CancellationToken cancellationToken)
            {
               return  Task.FromResult(_context.GetTransactionsPerEpoch(request.Epoch)); 
            }
        }
    }
}
