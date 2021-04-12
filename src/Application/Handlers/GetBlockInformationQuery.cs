using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers
{

    public record GetBlockNumberBySlotLeaderQuery(int SlotLeader) : IRequest<int>;

    public class GetBlockInformationQueryHandler : IRequestHandler<GetBlockNumberBySlotLeaderQuery, int>
    {
        private readonly IQueries _queries;

        public GetBlockInformationQueryHandler(IQueries queries)
        {
            _queries = queries;
        }
        public Task<int> Handle(GetBlockNumberBySlotLeaderQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_queries.GetBlockInformation(request.SlotLeader));
        }
    }

}
