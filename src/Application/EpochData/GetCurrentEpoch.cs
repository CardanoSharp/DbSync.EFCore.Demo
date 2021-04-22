using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
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
        public record GetCurrentEpochCommand() : IRequest<GetCurrentEpochResponse>;
        public class GetCurrentEpochHandler : IRequestHandler<GetCurrentEpochCommand, GetCurrentEpochResponse>
        {

            private readonly IQueries _context;
            private readonly ILogger<GetCurrentEpochResponse> _logger;

            public GetCurrentEpochHandler(IQueries context, ILogger<GetCurrentEpochResponse> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<GetCurrentEpochResponse> Handle(GetCurrentEpochCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var currentEpoch = await _context.GetCurrentEpoch();
                    return currentEpoch;
                }

                catch(NullReferenceException e)
                {
                    _logger.LogWarning("The attempt to return the current Epoch returned null on {Time}. Error message {Message}", DateTime.UtcNow, e);
                    throw;
                }

                catch(Exception e)
                {
                    _logger.LogError(e, "An unkown error occured when getting the current epoch on {Time}", DateTime.UtcNow);
                    throw; 
                }
                 
            }
        }

        public record GetCurrentEpochResponse(int CurrentEpoch);
    }
}
