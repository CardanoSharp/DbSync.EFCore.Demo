using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.EpochData
{
    public static class GetCurrentEpoch
    {
        public record Command() : IRequest<Respone>;

        public class Handler : IRequestHandler<Command, Respone>
        {

            private readonly IQueries _context;

            public Handler(IQueries context)
            {
                _context = context;

            }

            public async Task<Respone> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _context.GetCurrentEpoch(); 
            }
        }

        public record Respone(int CurrentEpoch);
    }
}
