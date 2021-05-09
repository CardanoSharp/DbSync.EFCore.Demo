using Application.Common.Interfaces;
using CardanoSharp.Wallet;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.WalletKeys
{
    public static class GetMnemonic
    {
        public record GetMnemonicDataCommand(int size) : IRequest<GetMnemonicDataResponse>;

        public class GetMnemonicDataHandler : IRequestHandler<GetMnemonicDataCommand, GetMnemonicDataResponse>
        {
            private readonly IKeyService _keyService;
            private readonly ILogger<GetMnemonicDataResponse> _logger;

            public GetMnemonicDataHandler(IKeyService keyService, ILogger<GetMnemonicDataResponse> logger)
            {
                _keyService = keyService;
                _logger = logger;
            }


            public async Task<GetMnemonicDataResponse> Handle(GetMnemonicDataCommand request, CancellationToken cancellationToken)
            {
                var preMnemonic = DateTime.UtcNow;
                var mnemonic = _keyService.Generate(request.size);
                var postMnemonic = DateTime.UtcNow;

                _logger.LogInformation($"The user requested a mnemonic of size {request.size}. Generated in {(preMnemonic - postMnemonic).TotalSeconds}");

                return new GetMnemonicDataResponse(mnemonic);
            }
        }

        public record GetMnemonicDataResponse(string Mnemonic);
    }
}
