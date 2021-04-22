using Application.Common.Interfaces;
using MediatR;
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

            public TransactionsInCurrentEpochHandler(IQueries context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }

            public async Task<List<TransactionsInEpochResponse>> Handle(TransactionsInCurrentEpochCommand request, CancellationToken cancellationToken)
            {
                var currentEpoch = await _mediator.Send(new GetCurrentEpochCommand(), cancellationToken);
                return await _context.GetTransactionsForUserEnteredEpoch(49); 
            }
        }
    }
}
