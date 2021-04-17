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
            private readonly IMapper _mapper;

            public Handler(IQueries context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public Task<List<Response>> Handle(Command request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_context.GetTransactionsPerEpoch(request.Epoch));
            }
        }

        public record Response(long Id, long TxInId, long TxOutId, short TxOutIndex ); 
    }
}
